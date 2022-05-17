using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.WebHost.UseUrls("http//*:7000");
builder.WebHost.ConfigureAppConfiguration(webBuilder =>
{
    webBuilder.AddJsonFile("ocelot.json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSwaggerForOcelotUI(options =>
//{
//    options.DownstreamSwaggerEndPointBasePath = "/swagger";
//    options.PathToSwaggerGenerator = "/swagger/docs";
//    options.SwaggerEndpoint("https://localhost:5201/swagger", "Authentication API");
//});

app.UseOcelot().Wait();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();