using System.Threading;
using System.Threading.Tasks;
using FixNGo.Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace FixNGo.Web.Controllers;

/// <summary>
/// Sample MVC Controller demonstrating full server-rendered Razor UI alongside domain repositories.
/// </summary>
public class ServiceCentersController : Controller
{
    private readonly IServiceCenterRepository _serviceCenterRepository;

    public ServiceCentersController(IServiceCenterRepository serviceCenterRepository)
    {
        _serviceCenterRepository = serviceCenterRepository;
    }

    // GET: /ServiceCenters
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var centers = await _serviceCenterRepository.GetAllAsync(cancellationToken);
        return View(centers);
    }

    // GET: /ServiceCenters/Details/5
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var center = await _serviceCenterRepository.GetByIdAsync(id, cancellationToken);
        if (center == null)
        {
            return NotFound();
        }

        return View(center);
    }
    // GET: /ServiceCenters/WorkingHours/5
    [HttpGet]
    public async Task<IActionResult> WorkingHours(int id, CancellationToken cancellationToken)
    {
        var center = await _serviceCenterRepository.GetByIdAsync(id, cancellationToken);
        if (center == null)
        {
            return NotFound();
        }

        return View(center.WorkingHours); 
    }
    // POST: /ServiceCenters/WorkingHours
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> WorkingHours(int id, List<FixNGo.Domain.Entities.WorkingHours> workingHours, CancellationToken cancellationToken)
    {
  
        TempData["Success"] = "Business hours have been successfully updated.!";
        return RedirectToAction(nameof(Details), new { id = id });
    }
}

