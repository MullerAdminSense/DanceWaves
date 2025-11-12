using Serilog;

namespace DanceWaves
{
    public static class SerilogConfig
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/dancewaves-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
