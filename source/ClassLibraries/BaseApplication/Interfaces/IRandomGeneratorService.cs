namespace BaseApplication.Interfaces;

public interface IRandomGeneratorService
{
    string GenerateNumericCode(int length = 6);
}