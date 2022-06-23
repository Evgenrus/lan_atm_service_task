using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();
//builder.Services.AddSwaggerForOcelot(builder.Configuration);

//builder.WebHost.UseUrls("http//*:7000");
builder.WebHost.ConfigureAppConfiguration(webBuilder =>
{
    webBuilder.AddJsonFile("ocelot.json");
    webBuilder.AddJsonFile("ocelot.Development.json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseOcelot().Wait();

//app.UseSwaggerForOcelotUI(options =>
//{
//    options.DownstreamSwaggerEndPointBasePath = "/swagger";
//    options.PathToSwaggerGenerator = "/swagger/docs";
//    options.SwaggerEndpoint("https://localhost:7000/swagger", "Authentication API");
//});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();