using ShiftFlow.Entities;

namespace ShiftFlow.Application.Common.Interfaces.Repositories
{
    public interface IUsersRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
    }
}
