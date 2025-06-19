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
    public UserService(
        DataContext context,
        IValidator<RequestUserDto> validator,
        IMapper mapper)
    {
        _context = context;
        _validator = validator;
        _mapper = mapper;
    }


    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="userDto"></param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Идентификатор</returns>
    public async Task<Guid> CreateUser(RequestUserDto userDto, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(userDto, token);

        var user = _mapper.Map<User>(userDto);
        var userPassword = PasswordHashService.Hash(userDto.Password);

        user.PasswordHash = userPassword.Password;
        user.PasswordSalt = userPassword.Salt;

        await _context.AddAsync(user, token);
        await _context.SaveChangesAsync(token);
        return user.Id;
    }
}
