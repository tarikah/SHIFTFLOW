using Microsoft.EntityFrameworkCore;
using ShiftFlow.Application.Common.Interfaces.Repositories;
using ShiftFlow.Entities;
using ShiftFlow.Infrastructure.Contexts;

namespace ShiftFlow.Infrastructure.Users;

public class UsersRepository : IUsersRepository
{
    private readonly ShiftFlowDbContext _dbContext;

    public UsersRepository(ShiftFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Add(ApplicationUser entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRange(List<ApplicationUser> entities)
    {
        throw new NotImplementedException();
    }

    public Task Delete(ApplicationUser entity)
    {
        throw new NotImplementedException();
    }

    public Task<List<ApplicationUser>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<ApplicationUser> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
    {
        return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == userName);

    }

    public Task Update(ApplicationUser entity)
    {
        throw new NotImplementedException();
    }
}

