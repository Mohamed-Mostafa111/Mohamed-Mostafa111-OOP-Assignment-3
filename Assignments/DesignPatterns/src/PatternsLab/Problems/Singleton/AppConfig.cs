 namespace PatternsLab.Problems.Singleton;

public class AppConfig
{
    public static int LoadCount;

    public string DbConnection { get; set; }
    public string Theme { get; set; }

    private static readonly Lazy<AppConfig> appConfig = new(() => new AppConfig());
    public static AppConfig AppConfig_Instance=> appConfig.Value;
    private AppConfig()
    {
		LoadCount++;
        Console.WriteLine($"[AppConfig] Loading settings from disk... (load #{LoadCount})");
        Thread.Sleep(300);
        DbConnection = "Server=localhost;Db=School";
        Theme = "Light";
    }
}

public class DatabaseService
{
    public AppConfig Config => AppConfig.AppConfig_Instance;

    public void Connect() => Console.WriteLine($"Connecting to {Config.DbConnection}");
}

public class UiService
{
    public AppConfig Config => AppConfig.AppConfig_Instance;

    public void Render() => Console.WriteLine($"UI is using theme: {Config.Theme}");
}
