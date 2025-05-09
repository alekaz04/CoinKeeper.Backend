using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Infrastructure;
using FluentValidation;

namespace CoinKeeper.Authentication;

public class UserService
{
    private readonly DataContext _context;
    private readonly IValidator<RequestUserDto> _validator;
    private readonly IMapper _mapper;
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
