using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;

namespace CoinKeeper.Finance;

/// <summary>
/// Круд хэндлер для сущности <see cref="Operation"/>
/// </summary>
public class OperationsCrudHandler : AbstractCrudHandler<Operation, OperationReadDto, OperationCreateDto>
{
    public OperationsCrudHandler(DataContext context, IMapper mapper, IValidator<OperationCreateDto> validator, ICurrentUser currentUser) : base(context, mapper, validator, currentUser)
    {
    }
}
