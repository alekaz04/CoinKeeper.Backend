# Cline Rules for CoinKeeper Project Analysis

## Analysis Report Guidelines

1. **Analysis Report Language**
   - Все отчеты об анализе кода должны быть написаны на РУССКОМ языке
   - Внутренний процесс анализа может проводиться на английском языке, но конечный отчет должен быть на русском
   - Использовать профессиональную терминологию, принятую в русскоязычном сообществе разработчиков .NET

2. **Analysis Report Format**
   - Имя файла должно соответствовать шаблону: `CoinKeeper_Analyse_DD_MM_YY.md`, где DD_MM_YY - дата анализа
   - В начале файла должна быть указана дата проведения анализа
   - Структурировать отчет с использованием заголовков и подзаголовков для лучшей читаемости

3. **Previous Analysis Review**
   - Перед проведением нового анализа необходимо изучить предыдущие отчеты в папке cline/analyse
   - Искать файлы по шаблону `CoinKeeper_Analyse_xx_xx_xx.md`, где xx_xx_xx - дата предыдущего анализа
   - Учитывать выводы и рекомендации из предыдущих анализов при составлении нового отчета
   - Отмечать прогресс в решении ранее выявленных проблем

## General Code Analysis Guidelines

1. **Code Organization Analysis**
   - Evaluate adherence to the established modular structure
   - Assess if related functionality is kept together
   - Check if namespaces appropriately reflect the module structure
   - Identify potential organizational improvements

2. **Coding Standards Analysis**
   - Verify compliance with C# coding conventions
   - Check for proper use of Pascal case for public members, classes, and properties
   - Verify correct use of camel case for private fields
   - Assess completeness of XML documentation comments on public APIs
   - Evaluate method size and complexity (methods should be < 30 lines when possible)

3. **Architecture Analysis**
   - Evaluate separation of concerns
   - Assess adherence to the established layering (API → Business Logic → Data Access)
   - Analyze dependency injection implementation
   - Check if controllers are kept thin with business logic delegated to handlers

4. **API Design Analysis**
   - Evaluate adherence to RESTful conventions
   - Verify appropriate use of HTTP methods (GET, POST, PUT, DELETE)
   - Check for appropriate status code usage
   - Assess consistency of response formats
   - Verify API documentation with XML comments for Swagger

5. **Database Access Analysis**
   - Evaluate Entity Framework Core usage patterns
   - Assess entity configurations in separate classes
   - Review migration strategies
   - Check for proper use of async/await for all database operations

## Code Pattern Analysis

### Entity Framework Usage Patterns

When analyzing Entity Framework usage, look for these patterns:

```csharp
// Querying pattern
var entity = await _context.Set<Entity>()
    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

// Creating pattern
var entity = _mapper.Map<Entity>(dto);
await _context.AddAsync(entity, cancellationToken);
await _context.SaveChangesAsync(cancellationToken);

// Updating pattern
var entity = await _context.Set<Entity>()
    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
_mapper.Map(dto, entity);
await _context.SaveChangesAsync(cancellationToken);

// Deleting pattern
var entity = await _context.Set<Entity>()
    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
_context.Remove(entity);
await _context.SaveChangesAsync(cancellationToken);
```

Identify deviations from these patterns and assess their impact.

### Controller Pattern Analysis

Look for this controller pattern and evaluate adherence:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ResourceController : CommonApiController
{
    private readonly ResourceHandler _handler;

    public ResourceController(ResourceHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id}")]
    public async Task<ResourceDto> GetResource(Guid id, CancellationToken cancellationToken)
    {
        return await _handler.GetResourceById(id, cancellationToken);
    }

    // Other endpoints...
}
```

### Handler Pattern Analysis

Evaluate handler implementations against this pattern:

```csharp
public class ResourceHandler
{
    private readonly IValidator<IResource> _validator;
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public ResourceHandler(IValidator<IResource> validator, IMapper mapper, DataContext context)
    {
        _validator = validator;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ResourceDto> GetResourceById(Guid id, CancellationToken cancellationToken)
    {
        var resource = await _context.Set<Resource>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (resource is null)
        {
            throw new CommonErrorException("Resource not found.");
        }

        return _mapper.Map<ResourceDto>(resource);
    }

    // Other methods...
}
```

### Validation Pattern Analysis

Check validation implementations against this pattern:

```csharp
public class ResourceValidator : AbstractValidator<IResource>
{
    public ResourceValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100)
            .WithMessage("Name is required and must be less than 100 characters");
        
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than or equal to 0");
        
        // Other rules...
    }
}
```

### Mapper Pattern Analysis

Evaluate mapper configurations against this pattern:

```csharp
public class ResourceMapper : Profile
{
    public ResourceMapper()
    {
        CreateMap<ResourceCreateDto, Resource>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()));

        CreateMap<ResourceUpdateDto, Resource>()
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore());

        CreateMap<Resource, ResourceDto>();
    }
}
```

## Code Analysis Workflow

1. **Feature Analysis**
   - Review requirements from project brief
   - Analyze API endpoint design and data model
   - Evaluate entity and configuration implementation
   - Review migration strategy
   - Assess DTOs and validation rules
   - Analyze mapper configuration
   - Evaluate handler logic
   - Review controller endpoint implementation
   - Assess testability with Swagger UI

2. **Code Review Checklist**
   - Verify adherence to established patterns
   - Assess validation completeness
   - Evaluate error handling robustness
   - Check for correct async/await usage
   - Verify proper dependency injection
   - Assess API documentation completeness
   - Evaluate RESTful convention adherence

3. **Testing Analysis**
   - Evaluate manual testing approach with Swagger UI
   - Assess unit test coverage for business logic
   - Review integration test strategy for API endpoints

## Improvement Opportunities Analysis

1. **API Routes**: Assess current route naming against RESTful conventions
2. **State Field**: Evaluate the ambiguity of the State field and potential renaming to OperationType as enum
3. **User Association**: Analyze the need for user entity and relationship to operations
4. **Validation**: Assess current validation rules and identify enhancement opportunities
5. **API Documentation**: Evaluate XML comments for Swagger completeness

## Documentation Analysis

1. **XML Comments**: Assess completeness of XML documentation on public APIs
2. **README**: Evaluate project overview, setup instructions, and usage examples
3. **API Documentation**: Review Swagger UI and endpoint descriptions
4. **Code Comments**: Assess commenting of complex logic

## Deployment Configuration Analysis

1. **Docker**: Evaluate Docker configuration for containerization
2. **Environment Variables**: Assess use of environment variables for configuration
3. **Database**: Review PostgreSQL configuration for production
4. **Logging**: Evaluate Serilog configuration for production logging
