using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace BlazorApp1.Client.Security
{
    //public class GenerateOutput : IClassFixture<AppTestFixture>, IDisposable
    //{
    //    private readonly AppTestFixture _fixture;
    //    private readonly HttpClient _client;
    //    private readonly string _outputPath;
    //    private readonly ITestOutputHelper _output;

    //    public GenerateOutput(AppTestFixture fixture, ITestOutputHelper output)
    //    {
    //        _fixture = fixture;
    //        _output = output;
    //        _fixture.Output = output;
    //        _client = fixture.CreateDefaultClient();

    //        var config = _fixture.Services.GetRequiredService<IConfiguration>();
    //        _outputPath = config["RenderOutputDirectory"];

    //        if (string.IsNullOrEmpty(_outputPath))
    //        {
    //            throw new ArgumentException("RenderOutputDirectory config value was null or empty", nameof(_outputPath));
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        _client?.Dispose();
    //        _fixture.Output = null;
    //    }

    //    /// <summary>
    //    /// Massage the values into something that works for xunit theory
    //    /// </summary>
    //    public static IEnumerable<object[]> GetPagesToPreRender()
    //        => PrerenderRouteHelper
    //            .GetRoutes(typeof(BlazorApp1.App).Assembly)
    //            .Select(config => new object[] { config });

    //    [Theory, Trait("Category", "PreRender")]
    //    [MemberData(nameof(GetPagesToPreRender))]
    //    public async Task Render(string route)
    //    {
    //        // strip the initial / off
    //        var renderPath = route.Substring(1);

    //        var relativePath = Path.Combine(_outputPath, renderPath);
    //        var outputDirectory = Path.GetFullPath(relativePath);

    //        _output.WriteLine($"Creating directory '{outputDirectory}'");
    //        Directory.CreateDirectory(outputDirectory);

    //        var fileName = Path.Combine(outputDirectory, "index.html");

    //        _output.WriteLine($"Fetching prerendered content for '{route}'");
    //        var result = await _client.GetStreamAsync(route);

    //        _output.WriteLine($"Writing content to '{fileName}'");
    //        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
    //        {
    //            await result.CopyToAsync(file);
    //        }

    //        _output.WriteLine($"Pre rendering complete");

    //    }
    //}


    public static class PrerenderRouteHelper
    {
        public static List<string> GetRoutesToRender(Assembly assembly)
        {
            // Get all the components whose base class is ComponentBase
            var components = assembly
                .ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(ComponentBase)));

            var routes = components
                .Select(component => GetRouteFromComponent(component))
                .Where(config => config is not null)
                .ToList();

            return routes;
        }

        private static string GetRouteFromComponent(Type component)
        {
            var attributes = component.GetCustomAttributes(inherit: true);

            var routeAttribute = attributes.OfType<RouteAttribute>().FirstOrDefault();

            if (routeAttribute is null)
            {
                // Only map routable components
                return null;
            }

            var route = routeAttribute.Template;

            if (string.IsNullOrEmpty(route))
            {
                throw new Exception($"RouteAttribute in component '{component}' has empty route template");
            }

            // Doesn't support tokens yet
            //if (route.Contains('{'))
            //{
            //    throw new Exception($"RouteAttribute for component '{component}' contains route values. Route values are invalid for prerendering");
            //}

            var sss = route.Split("/", StringSplitOptions.RemoveEmptyEntries);


            return route;
        }
    }
}
