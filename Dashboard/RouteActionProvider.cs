using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Dashboard;

public class RouteActionProvider
{
    private readonly IEndpointRouteBuilder _builder;
    private readonly DashboardOptions _options;

    public RouteActionProvider(IEndpointRouteBuilder builder, DashboardOptions options)
    {
        _builder = builder;
        _options = options;
    }

    public void MapDashboardRoutes()
    {
        var prefixMatch = _options.PathMatch + "/api";
        _builder.MapGet(prefixMatch + "/health", Health).AllowAnonymous();
    }


    public Task Health(HttpContext httpContext)
    {
        //当前服务器时间
        var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        httpContext.Response.WriteAsync($"Health check at {now}");
        return Task.CompletedTask;
    }
}