using PilotFlow.Api.Endpoints;
using PilotFlow.Api.Middleware;
using PilotFlow.Application;
using PilotFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("frontend");
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapAccessRequestEndpoints();
app.MapTaskEndpoints();
app.MapAuditEndpoints();

app.Run();
