using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FixNGo.Application.Abstractions.Persistence;
using FixNGo.Domain.Entities;
using FixNGo.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FixNGo.Infrastructure.Persistence.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _context;

    public ProviderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Provider, cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicationUser>> GetActiveProvidersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.Provider && u.IsActive && !u.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}

