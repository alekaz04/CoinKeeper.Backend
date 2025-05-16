using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Infrastructure;
using FluentValidation;

namespace CoinKeeper.Authentication;

/// <summary>
/// Сервис пользователей
/// </summary>
public class UserService
{
    /// <inheritdoc cref="DataContext"/>
    private readonly DataContext _context;

    /// <inheritdoc cref="IValidator"/>
    private readonly IValidator<RequestUserDto> _validator;

    /// <inheritdoc cref="IMapper"/>
    private readonly IMapper _mapper;

    /// <inheritdoc cref="PasswordHashService"/>
    private readonly PasswordHashService _passwordHashService;

    public UserService(
        DataContext context,
        IValidator<RequestUserDto> validator,
        IMapper mapper,
        PasswordHashService passwordHashService)
    {
        _context = context;
        _validator = validator;
        _mapper = mapper;
        _passwordHashService = passwordHashService;
    }


    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="userDto"></param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Идентификатор</returns>
    public async Task<Guid> CreateUser(RequestUserDto userDto, CancellationToken token)
    {
        _validator.ValidateAndThrow(userDto);

        var user = _mapper.Map<User>(userDto);
        var userPassword = _passwordHashService.Hash(userDto.Password);

        user.PasswordHash = userPassword.Password;
        user.PasswordSalt = userPassword.Salt;

        await _context.AddAsync(user, token);
        await _context.SaveChangesAsync(token);
        return user.Id;
    }
}
