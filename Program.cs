using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using Data;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()                     
              .AllowAnyMethod();                    
    });
});

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<KPZContext>(options => {
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();
builder.Services.AddScoped<IMatchmakingService, MatchmakingService>();
builder.Services.AddScoped<InferenceEngineService>();

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAngularDevClient");

app.MapControllers();
app.Run();
