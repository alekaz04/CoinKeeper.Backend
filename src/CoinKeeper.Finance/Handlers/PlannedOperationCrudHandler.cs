using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;

namespace CoinKeeper.Finance;

public class PlannedOperationCrudHandler : AbstractCrudHandler<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto>
{
    public PlannedOperationCrudHandler(DataContext context, IMapper mapper, IValidator<PlannedOperationCreateDto> validator, ICurrentUser currentUser) : base(context, mapper, validator, currentUser)
    {
    }
}
