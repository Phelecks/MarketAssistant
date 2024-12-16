namespace BaseInfrastructure.Common;

public interface IApplicationDbContextInitializer
{
    Task InitialiseAsync();
    Task SeedAsync();
}