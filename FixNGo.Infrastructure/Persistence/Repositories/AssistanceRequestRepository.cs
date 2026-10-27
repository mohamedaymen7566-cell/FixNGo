using System.Threading;
using System.Threading.Tasks;
using FixNGo.Application.Abstractions.Persistence;

namespace FixNGo.Infrastructure.Persistence.Repositories;

public class AssistanceRequestRepository : IAssistanceRequestRepository
{
    private readonly AppDbContext _context;

    public AssistanceRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<object?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<object?>(null);
    }
}

