using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Finance;

/// <summary>
/// Хэндлер баланаса счёта
/// </summary>
public class BalanceHandler : IBalanceHandler
{
    /// <inheritdoc cref="DataContext"/>
    private readonly DataContext _context;

    /// <inheritdoc cref="ICurrentUser"/>
    private readonly ICurrentUser _currentUser;

    /// <inheritdoc cref="IMapper"/>
    private readonly IMapper _mapper;

    public BalanceHandler(DataContext context, ICurrentUser currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<BalanceUserDto> GetCurrentUserBalance(CancellationToken token)
    {
        var userId = _currentUser.GetCurrentUserId();

        var accounts = await _context.Set<Account>()
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .ProjectTo<AccountBalanceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        decimal totalBalance = accounts.Sum(x => x.Balance);

        return new BalanceUserDto() { AccountBalances = accounts, Balance = totalBalance };
    }
}
