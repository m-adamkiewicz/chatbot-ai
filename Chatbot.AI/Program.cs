using Chatbot.AI.Api.DAL;
using Chatbot.AI.Api.Handlers;
using Chatbot.AI.Api.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddSingleton<ICancellationService, CancellationService>();
builder.Services.AddTransient<IChatHub, ChatHub>();
builder.Services.AddTransient<IArtificialIntelligenceService, ArtificialIdiotService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddDbContext<ChatbotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CorsPolicy");
app.MapHub<ChatHub>("/chat-hub");

app.MapGet("/messages", (IMediator mediator, CancellationToken cancellationToken) => mediator.Send(new GetMessages(), cancellationToken));
app.MapPost("/messages", (SendMessage message, IMediator mediator, CancellationToken cancellationToken) => mediator.Send(message, cancellationToken));
app.MapPut("/messages/{id}/start", (Guid id, StartMessage startMessage, IMediator mediator, CancellationToken cancellationToken) => mediator.Send(startMessage with { MessageId = id }, cancellationToken));
app.MapPut("/messages/{id}/cancel", (Guid id, IMediator mediator, CancellationToken cancellationToken) => mediator.Send(new CancelMessage(id), cancellationToken));
app.MapPut("/messages/{id}/react", (Guid id, ReactMessage msg, IMediator mediator, CancellationToken cancellationToken) => mediator.Send(msg with { Id = id }, cancellationToken));

app.Run();