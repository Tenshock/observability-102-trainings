namespace Greetings;

public class Greetings
{
    public async Task<String> SayHi()
    {
        var random = new Random();
        int randomNumber = random.Next(500, 1001);
        await Task.Delay(randomNumber);

        return "Hello, world! 🦝";
    }
}
