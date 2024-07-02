using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Logging;

public static partial class ProgramExtensions
{
  public static void Configure(this WebApplication app)
  {

    if (app.Environment.IsDevelopment())
    {
      IdentityModelEventSource.ShowPII = true;
    }

    app.UseDefaultExceptionHandler(logStructuredException: true);

    app.UseRouting();

    app.UseCors();

    app.Use(async (context, next) =>
    {
      context.Request.EnableBuffering();

      await next();

    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseFastEndpoints();

    app.UseStaticFiles();
    app.UseCookiePolicy();

    app.UseSwaggerGen(config =>
    {
      config.PostProcess = (document, _) =>
      {
        document.BasePath = "/";
      };
    });


    app.MapDefaultControllerRoute();

  }
}
