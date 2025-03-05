using Microsoft.Extensions.Logging;

namespace Greetings;

public class Greetings
{
    private readonly ILogger<Greetings> _logger;

    public Greetings(ILogger<Greetings> logger)
    {
        _logger = logger;
    }

    public async Task<String> SayHi()
    {
        _logger.LogInformation("I'll send you pandas. 🐼");

        var random = new Random();
        int randomNumber = random.Next(500, 1001);
        await Task.Delay(randomNumber);

        return "Hello, world! 🦝";
    }
}
