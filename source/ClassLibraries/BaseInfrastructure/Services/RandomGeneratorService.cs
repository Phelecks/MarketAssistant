using System.Text;
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
        var codeBuilder = new StringBuilder();
        for (var i = 0; i < length; i++)
            codeBuilder.Append(_random.Next(minValue: 0, maxValue: 10));
        return codeBuilder.ToString();
    }
}