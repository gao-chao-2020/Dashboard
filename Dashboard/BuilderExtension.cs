using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using System.Text;

namespace Dashboard;

public static class DashboardBuilderExtension
{
    private const string EmbeddedFileNamespace = "Dashboard.wwwroot.dist";

    public static IApplicationBuilder UseDashboard(this IApplicationBuilder app, DashboardOptions? options = null)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        var provider = app.ApplicationServices;
        if (options == null) options = new DashboardOptions();
        app.UseStaticFiles(new StaticFileOptions
        {
            RequestPath = options.PathMatch,
            FileProvider = new EmbeddedFileProvider(options.GetType().Assembly, EmbeddedFileNamespace)
        });

        var endpointRouteBuilder = (IEndpointRouteBuilder)app.Properties["__GlobalEndpointRouteBuilder"]!;

        endpointRouteBuilder.MapGet(
            pattern: options.PathMatch,
            requestDelegate: httpContext =>
        {
            var path = httpContext.Request.Path.Value;

            var redirectUrl = string.IsNullOrEmpty(path) || path.EndsWith("/")
                ? "index.html"
                : $"{path.Split('/').Last()}/index.html";

            httpContext.Response.StatusCode = 301;
            httpContext.Response.Headers["Location"] = redirectUrl;
            return Task.CompletedTask;
        });

        endpointRouteBuilder.MapGet(
            pattern: options.PathMatch + "/index.html",
            requestDelegate: async httpContext =>
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "text/html;charset=utf-8";

            await using var stream = options.GetType().Assembly.GetManifestResourceStream(EmbeddedFileNamespace + ".index.html");

            if (stream == null) throw new InvalidOperationException();

            using var sr = new StreamReader(stream);
            var htmlBuilder = new StringBuilder(await sr.ReadToEndAsync());
            htmlBuilder.Replace("%(servicePrefix)", options.PathBase + options.PathMatch + "/api");
            htmlBuilder.Replace("%(pollingInterval)", options.StatsPollingInterval.ToString());
            await httpContext.Response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        });

        new RouteActionProvider(endpointRouteBuilder, options).MapDashboardRoutes();

        return app;
    }
}