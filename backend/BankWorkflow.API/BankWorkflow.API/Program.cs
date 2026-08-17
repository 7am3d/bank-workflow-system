using BankWorkflow.API.Data;
using BankWorkflow.API.Middleware;
using BankWorkflow.API.Repositories.Implementations;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Implementations;
using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Database
// ==========================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================
// Dependency Injection
// ==========================

builder.Services.AddHttpContextAccessor();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestTypeRepository, RequestTypeRepository>();
builder.Services.AddScoped<IWorkflowRequestRepository, WorkflowRequestRepository>();
builder.Services.AddScoped<IWorkflowStepRepository, WorkflowStepRepository>();
builder.Services.AddScoped<IWorkflowHistoryRepository, WorkflowHistoryRepository>();
builder.Services.AddScoped<IWorkflowCommentRepository, WorkflowCommentRepository>();
builder.Services.AddScoped<IWorkflowAttachmentRepository, WorkflowAttachmentRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRequestTypeService, RequestTypeService>();
builder.Services.AddScoped<IWorkflowRequestService, WorkflowRequestService>();
builder.Services.AddScoped<IWorkflowApprovalService, WorkflowApprovalService>();
builder.Services.AddScoped<IWorkflowHistoryService, WorkflowHistoryService>();
builder.Services.AddScoped<IWorkflowCommentService, WorkflowCommentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IWorkflowAttachmentService, WorkflowAttachmentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
// ==========================
// JWT Authentication
// ==========================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("JWT Key is not configured.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(jwtKey))
    };
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// ==========================
// Swagger
// ==========================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bank Workflow API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT token below (without 'Bearer')."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==========================
// Database Initialization
// ==========================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(context);
}

// ==========================
// HTTP Pipeline
// ==========================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();