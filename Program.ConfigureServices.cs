using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http.Features;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;


public static partial class ProgramExtensions
{
  public static void ConfigureServices(this WebApplicationBuilder builder)
  {
    builder.ConfigureCrossCuttingConcerns();


    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
      options.CheckConsentNeeded = context => true;
      options.MinimumSameSitePolicy = SameSiteMode.None;
    });

  
    builder.Services.AddRazorPages();

    builder.ConfigureFastEndpoints();
   
    builder.WebHost.ConfigureKestrel(options =>
    {
      options.Limits.MaxRequestBodySize = 1073741824;
    });

    builder.Services.Configure<FormOptions>(options =>
    {
      options.MultipartBodyLengthLimit = 1073741824; 
    });


  }

  private static void ConfigureCrossCuttingConcerns(this WebApplicationBuilder builder)
  {
    builder.Services.AddCors(options =>
    {
      options.AddDefaultPolicy(
          builder =>
          {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
          });
    });

  }

  private static void ConfigureFastEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddFastEndpoints();
    

    builder.Services.AddControllers()
      .ConfigureApiBehaviorOptions(options =>
      {
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
          var logger = context.HttpContext.RequestServices
                              .GetRequiredService<ILogger<Program>>();

          logger.LogWarning("ModelState is invalid: {ModelState}", context.ModelState);

          return builtInFactory(context);
        };
      });

    builder.Services.SwaggerDocument(o =>
    {
      o.EnableJWTBearerAuth = false;

      o.DocumentSettings = s =>
      {
        s.OperationProcessors.Add(new RemoveExamplesOperationFilter());
        s.DocumentName = "swagger";
        s.Title = "MyWebApi";
        s.Version = "v1.0";
        s.Description = "Apis for WebApi";
        s.SchemaSettings.SchemaType = NJsonSchema.SchemaType.Swagger2;
   
      };
    });

    builder.Services.SwaggerDocument(o =>
    {
      o.DocumentSettings = s =>
      {
        s.DocumentName = "openapi";
        s.Title = "MyWebApi";
        s.Description = "Apis coming under MyWebApi";
        s.SchemaSettings.SchemaType = NJsonSchema.SchemaType.OpenApi3;
      };
    });
  }

}

public class RemoveExamplesOperationFilter : IOperationProcessor
{
  public bool Process(OperationProcessorContext context)
  {
    foreach (var response in context.OperationDescription.Operation.Responses.Where(r => r.Value.Examples != null).ToList())
    {
      response.Value.Examples = null;
    }
    return true;
  }
}
