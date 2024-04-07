var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    await next.Invoke();

    var path = context.Request.Path;
    const string nameLogFile = "log.txt";

    using StreamWriter sw = new StreamWriter(nameLogFile, true);
    
    sw.WriteLine($"{path}: {(context.Response.StatusCode == 200 ? "Ok" : "Bad request" )}");
    sw.Close();
});

app.UseMiddleware<SaveRequestInFileMiddleware>();


app.Run();
