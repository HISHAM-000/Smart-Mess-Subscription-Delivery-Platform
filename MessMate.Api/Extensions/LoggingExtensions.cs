using Serilog;

namespace MessMate.Api.Extensions
{
    public static class LoggingExtensions
    {
        public static void AddSerilogLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
