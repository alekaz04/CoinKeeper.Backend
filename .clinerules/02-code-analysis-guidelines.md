# Code Analysis Guidelines for CoinKeeper Project

## Brief overview
This set of guidelines outlines how to conduct and document code analysis for the CoinKeeper project. Analysis reports should be written in Russian, while the internal analysis process can be conducted in English. These guidelines cover report formatting, code organization, coding standards, architecture, API design, database access, and more.

## Analysis report language and format
- All code analysis reports must be written in Russian language (Русский язык)
- Internal analysis process can be conducted in English, but the final report must be in Russian
- Use professional terminology accepted in the Russian-speaking .NET developer community
- File naming convention: `CoinKeeper_Analyse_DD_MM_YY.md` where DD_MM_YY is the analysis date
- Include the analysis date at the beginning of the file
- Structure the report with headings and subheadings for better readability
- Before conducting a new analysis, review previous reports in the cline/analyse folder
- Look for files matching the pattern `CoinKeeper_Analyse_xx_xx_xx.md`
- Consider findings and recommendations from previous analyses when creating a new report
- Note progress in resolving previously identified issues

## Code organization analysis
- Evaluate adherence to the established modular structure
- Assess if related functionality is kept together
- Check if namespaces appropriately reflect the module structure
- Identify potential organizational improvements

## Coding standards analysis
- Verify compliance with C# coding conventions
- Check for proper use of Pascal case for public members, classes, and properties
- Verify correct use of camel case for private fields
- Assess completeness of XML documentation comments on public APIs
- Evaluate method size and complexity (methods should be < 30 lines when possible)

## Architecture analysis
- Evaluate separation of concerns
- Assess adherence to the established layering (API → Business Logic → Data Access)
- Analyze dependency injection implementation
- Check if controllers are kept thin with business logic delegated to handlers

## API design analysis
- Evaluate adherence to RESTful conventions
- Verify appropriate use of HTTP methods (GET, POST, PUT, DELETE)
- Check for appropriate status code usage
- Assess consistency of response formats
- Verify API documentation with XML comments for Swagger

## Database access analysis
- Evaluate Entity Framework Core usage patterns
- Assess entity configurations in separate classes
- Review migration strategies
- Check for proper use of async/await for all database operations

## Analysis workflow
- Feature Analysis:
  - Review requirements from project brief
  - Analyze API endpoint design and data model
  - Evaluate entity and configuration implementation
  - Review migration strategy
  - Assess DTOs and validation rules
  - Analyze mapper configuration
  - Evaluate handler logic
  - Review controller endpoint implementation
  - Assess testability with Swagger UI
- Code Review:
  - Verify adherence to established patterns
  - Assess validation completeness
  - Evaluate error handling robustness
  - Check for correct async/await usage
  - Verify proper dependency injection
  - Assess API documentation completeness
  - Evaluate RESTful convention adherence
- Testing Analysis:
  - Evaluate manual testing approach with Swagger UI
  - Assess unit test coverage for business logic
  - Review integration test strategy for API endpoints

## Improvement opportunities
- API Routes: Assess current route naming against RESTful conventions
- State Field: Evaluate the ambiguity of the State field and potential renaming to OperationType as enum
- User Association: Analyze the need for user entity and relationship to operations
- Validation: Assess current validation rules and identify enhancement opportunities
- API Documentation: Evaluate XML comments for Swagger completeness

## Documentation and deployment
- Documentation:
  - Assess completeness of XML documentation on public APIs
  - Evaluate project overview, setup instructions, and usage examples
  - Review Swagger UI and endpoint descriptions
  - Assess commenting of complex logic
- Deployment Configuration:
  - Evaluate Docker configuration for containerization
  - Assess use of environment variables for configuration
  - Review PostgreSQL configuration for production
  - Evaluate Serilog configuration for production logging
