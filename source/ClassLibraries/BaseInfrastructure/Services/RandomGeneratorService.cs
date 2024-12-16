using BaseApplication.Interfaces;

namespace BaseInfrastructure.Services;

public class RandomGeneratorService : IRandomGeneratorService
{
    private readonly Random _random;

    public RandomGeneratorService()
    {
        _random = new Random();
    }

    public string GenerateNumericCode(int length = 6)
    {
        var code = string.Empty;
        for (var i = 0; i < length; i++)
            code += _random.Next(minValue: 0, maxValue: 10);
        return code;
    }
}