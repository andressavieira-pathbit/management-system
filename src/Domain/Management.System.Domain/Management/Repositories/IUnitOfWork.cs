using Microsoft.EntityFrameworkCore.Storage;

namespace Management.System.Domain.Management.Repositories;

public interface IUnitOfWork
{
    IDbContextTransaction BeginTransaction();
}
