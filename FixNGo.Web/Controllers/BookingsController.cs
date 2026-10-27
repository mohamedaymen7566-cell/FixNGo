using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FixNGo.Infrastructure.Persistence;
using FixNGo.Domain.Entities;

namespace FixNGo.Web.Controllers;

public class BookingsController : Controller
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Bookings/Create?serviceCenterId=1
    [HttpGet]
    public async Task<IActionResult> Create(int serviceCenterId)
    {
        // Retrieve the service center along with its offered services
        var serviceCenter = await _context.ServiceCenters
            .Include(sc => sc.Services)
            .FirstOrDefaultAsync(sc => sc.Id == serviceCenterId);

        if (serviceCenter == null)
        {
            return NotFound();
        }

        // Pass center name and services to the view via ViewBag
        ViewBag.ServiceCenterName = serviceCenter.Name;
        ViewBag.Services = serviceCenter.Services;

        // Initialize a new booking instance with default values
        var booking = new Booking
        {
            ServiceCenterId = serviceCenterId,
            AppointmentDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
        };

        return View("~/Views/ServiceCenters/Create.cshtml", booking);
    }

    // POST: Bookings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        // Remove navigation properties from ModelState validation to avoid validation errors
        ModelState.Remove(nameof(Booking.ServiceCenter));
        ModelState.Remove(nameof(Booking.Service));
        ModelState.Remove(nameof(Booking.Customer));

        if (ModelState.IsValid)
        {
            // Add the new booking to the database context and save changes
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ServiceCenters");
        }

        // Reload service center data if model state is invalid
        var serviceCenter = await _context.ServiceCenters
            .Include(sc => sc.Services)
            .FirstOrDefaultAsync(sc => sc.Id == booking.ServiceCenterId);

        ViewBag.ServiceCenterName = serviceCenter?.Name;
        ViewBag.Services = serviceCenter?.Services;

        return View("~/Views/ServiceCenters/Create.cshtml", booking);
    }
}