using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;


namespace CoinKeeper.Extensions.DependencyInjection;

public static class CommonApplicationBuilderExtensions
{
    public static IApplicationBuilder UseScalar(this IApplicationBuilder app)
    {

        return app;
    }
}
