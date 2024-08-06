using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Trello.API.Configurations;
using Trello.Application;
using Trello.Application.Utilities.Helper.SignalRHub;
using Trello.Application.Utilities.Middleware;
using Trello.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

var apiCorsPolicy = "ApiCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
        builder =>
        {
            builder.WithOrigins("https://localhost:3000", "http://localhost:3000", "http://127.0.0.1:5500")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddDbContext<TrellocloneContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
});
builder.Services.RegisterJwtModule(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.RegisterSwaggerModule();
builder.Services.InfrastructureRegister();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("clonetrello-103ad-firebase-adminsdk-plg5l-9823b9f478.json")
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionMiddleware();
app.UseApplicationSwagger();
app.UseApplicationJwt();
app.UseCors(apiCorsPolicy);
app.UseHttpsRedirection();
app.MapHub<SignalHub>("/signalHub");

app.UseAuthorization();

app.MapControllers();

app.Run();

