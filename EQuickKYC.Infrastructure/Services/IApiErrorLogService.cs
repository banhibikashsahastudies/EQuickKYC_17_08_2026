using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ApiErrorLogService : IApiErrorLogService
{
    private readonly EQuickKYCDbContext _dbContext;

    public ApiErrorLogService(EQuickKYCDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApiErrorLog>> GetApiErorLogs()
    {
        var errorLog = await _dbContext.ApiErrorLogs.ToListAsync();
        if (errorLog.Count == 0) return errorLog;

        return errorLog;
    }

    public async Task LogAsync(ApiErrorLog errorLog)
    {
        _dbContext.ApiErrorLogs.Add(errorLog);

        await _dbContext.SaveChangesAsync();
    }
}