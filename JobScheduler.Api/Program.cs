using JobScheduler.Api.Extentions;
using JobScheduler.Infrastructure.DependencyInjection.DapperClient.Bootstrap;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices(builder.Configuration);



var app = builder.Build();

app.Services.GetRequiredService<IDatabaseBootstrap>().Setup();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();