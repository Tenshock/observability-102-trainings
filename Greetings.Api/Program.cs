var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet(
    "/hello-world",
    async () =>
    {
        var greetings = new Greetings.Greetings();

        return await greetings.SayHi();
    }
);

app.Run();
