using FastEndpoints;
 
namespace MyWebApi.Endpoints.WeatherEndpoints;
 
public class Get : Endpoint<GetRequest>
{
  public override void Configure()
  {
    Get("/weather/{Facility}");
    AllowAnonymous();
    Description(b => b.WithName("Weather.Get"));
    Summary(s =>
    {
      s.Summary = "Get weather";
      s.Description = "Get weather";
    });
 
  }
 
  public override async Task HandleAsync(GetRequest request, CancellationToken ct)
  {
    await SendOkAsync(request.Facility);
  }
}