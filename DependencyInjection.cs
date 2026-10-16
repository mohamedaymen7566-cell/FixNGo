using FixNGo.Application.Abstractions.Integrations;
using FixNGo.Application.Abstractions.Notifications;
using FixNGo.Application.Abstractions.Persistence;
using FixNGo.Application.Features.Accounts;
using FixNGo.Application.Features.Assistance;
using FixNGo.Application.Features.Bookings;
using FixNGo.Application.Features.Payments;
using FixNGo.Application.Features.Reviews;
using FixNGo.Application.Features.Tracking;
using FixNGo.Application.Features.Vehicles;
using FixNGo.Application.Features.Verification;
using FixNGo.Domain.Entities;
using FixNGo.Infrastructure.BackgroundWorkers;
using FixNGo.Infrastructure.Integrations;
using FixNGo.Infrastructure.Persistence;
using FixNGo.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FixNGo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Persistence - SQL Server
        string connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=(localdb)\\mssqllocaldb;Database=FixNGoDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        // 2. Repositories
        services.AddScoped<IAssistanceRequestRepository, AssistanceRequestRepository>();
        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IServiceCenterRepository, ServiceCenterRepository>();

        // 3. Integrations
        services.AddSingleton<IMapService, GoogleMapsAdapter>();
        services.AddSingleton<IPaymentGateway, PaymentGatewayAdapter>();
        services.AddSingleton<INotificationService, NotificationAdapter>();
        services.AddSingleton<IFileStorageService, LocalFileStorageService>();

        // 4. Background Workers
        services.AddHostedService<OutboxDispatcherBackgroundService>();

        // 5. Application Services
        services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IProviderMatchingService, ProviderMatchingService>();
        services.AddScoped<IDispatchService, DispatchService>();
        services.AddScoped<IAssistanceRequestService, AssistanceRequestService>();
        services.AddScoped<ITrackingService, TrackingService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IVerificationService, VerificationService>();

        return services;
    }
}
