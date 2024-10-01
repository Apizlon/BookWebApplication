using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Добавляем поддержку консоли

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ApiGateway.Router>(new ApiGateway.Router("routes.json"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Get logger instance
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Intercept all incoming requests
app.Use(async (context, next) =>
{
    logger.LogInformation($"Received request: {context.Request.Method} {context.Request.Path}");

    var router = app.Services.GetRequiredService<ApiGateway.Router>();
    var responseMessage = await router.RouteRequest(context.Request);

    if (responseMessage.IsSuccessStatusCode)
    {
        var content = await responseMessage.Content.ReadAsStringAsync();
        context.Response.StatusCode = (int)responseMessage.StatusCode;
        await context.Response.WriteAsync(content);
    }
    else
    {
        // Handle error response from the router
        context.Response.StatusCode = (int)responseMessage.StatusCode;
        await context.Response.WriteAsync("Error while routing the request");
    }

    // Optionally invoke next middleware (if there are other middleware in the pipeline)
    // If you don't have other middleware that needs to run after this, you can remove next() invocation.
    await next();  // Pass the request to the next middleware in the pipeline, if needed.
});

app.Run();
