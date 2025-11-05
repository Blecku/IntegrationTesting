using Testcontainers.MsSql;

public class SharedMsSqlContainer
{
    private static readonly Lazy<MsSqlContainer> Container = new(CreateContainer);
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    private static bool _isStarted = false;

    private static MsSqlContainer CreateContainer()
    {
        return new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("YourStrong@Passw0rd")
            .WithPortBinding(1433, true)
            .WithName($"test-mssql-{Guid.NewGuid()}")
            .Build();
    }

    public static async Task<MsSqlContainer> GetContainerAsync()
    {
        await Semaphore.WaitAsync();
        try
        {
            if (!_isStarted)
            {
                await Container.Value.StartAsync();
                _isStarted = true;
            }
            return Container.Value;
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public static string GetConnectionString() => Container.Value.GetConnectionString();
}