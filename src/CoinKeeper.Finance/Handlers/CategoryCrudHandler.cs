using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;

namespace CoinKeeper.Finance;

public class CategoryCrudHandler : AbstractCrudHandler<Category, CategoryReadDto, CategoryCreateDto>
{
    public CategoryCrudHandler(DataContext context, IMapper mapper, IValidator<CategoryCreateDto> validator, ICurrentUser currentUser) : base(context, mapper, validator, currentUser)
    {
    }
}
