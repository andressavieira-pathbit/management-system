using Management.System.Domain.Management.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Management.System.Infrastructure.Repository;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }
}

