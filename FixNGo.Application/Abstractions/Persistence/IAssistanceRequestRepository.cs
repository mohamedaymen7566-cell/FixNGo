using System.Threading;
using System.Threading.Tasks;

namespace FixNGo.Application.Abstractions.Persistence;

/// <summary>
/// Repository contract for emergency roadside assistance requests.
/// </summary>
public interface IAssistanceRequestRepository
{
    Task<object?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

