using AutoMapper;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using CoinKeeper.Operations.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Operations.Handlers;

public class OperationsCrudHandler
{
    private readonly IValidator<OperationCreateDto> _validator;
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public OperationsCrudHandler(IValidator<OperationCreateDto> validator, IMapper mapper, DataContext context)
    {
        _validator = validator;
        _mapper = mapper;
        _context = context;
    }

    public async Task<Guid> CreateOperation(OperationCreateDto dto, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(dto);
        var operation = _mapper.Map<Operation>(dto);
        await _context.AddAsync(operation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return operation.Id;
    }

    public async Task<OperationReadDto> GetOperationById(Guid id, CancellationToken cancellationToken)
    {
        var operation = await _context.Set<Operation>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (operation is null)
        {
            throw new CommonErrorException("Operation not found.");
        }

        return _mapper.Map<OperationReadDto>(operation);
    }
}
