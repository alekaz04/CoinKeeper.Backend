using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Finance;

/// <summary>
/// Круд хэндлер для сущности <see cref="Operation"/>
/// </summary>
public class OperationsCrudHandler : AbstractCrudHandler<Operation, OperationReadDto, OperationCreateDto, OperationUpdateDto>
{
    /// <inheritdoc cref="IAccountBalanceService"/>
    private readonly IAccountBalanceService _accountBalanceService;

    /// <inheritdoc cref="DataContext"/>
    private readonly DataContext _context;

    public OperationsCrudHandler(IAccountBalanceService accountBalanceService, DataContext context, IMapper mapper, IValidator<OperationCreateDto> validator, ICurrentUser currentUser) : base(context, mapper, validator, currentUser)
    {
        _accountBalanceService = accountBalanceService;
        _context = context;
    }

    public override async Task<Guid> Create(OperationCreateDto createDto, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);

        try
        {
            var operationId = await base.Create(createDto, token);

            var operation = await _context.Set<Operation>()
                .FirstOrDefaultAsync(o => o.Id == operationId, token);

            await _accountBalanceService.ApplyOperationToBalance(operation!, token);

            await transaction.CommitAsync(token);

            return operationId;
        }
        catch
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }

    public override async Task Update(Guid id, OperationUpdateDto entityDto, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);

        try
        {
            await base.Update(id, entityDto, token);

            var accountId = await _context.Set<Operation>()
                .AsNoTracking()
                .Include(x => x.Account)
                .Where(x => x.Id == id)
                .Select(x => x.Account!.Id)
                .FirstOrDefaultAsync(token);

            await _accountBalanceService.UpdateAccountBalance(accountId, token);

            await transaction.CommitAsync(token);
        }
        catch
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }

    public override async Task Delete(Guid id, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);

        try
        {
            await base.Delete(id, token);

            var operation = await _context.Set<Operation>()
                .FirstAsync(o => o.Id == id, token);

            await _accountBalanceService.UpdateAccountBalance(operation.AccountId, token);

            await transaction.CommitAsync(token);
        }
        catch
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }
}
