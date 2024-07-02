public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureServices();

        var app = builder.Build();

        using (app.Logger.BeginScope("{Action} {Identifier}", "StartUp", "Main"))
        {
            app.Configure();

            app.Run();
        }
    }
}
