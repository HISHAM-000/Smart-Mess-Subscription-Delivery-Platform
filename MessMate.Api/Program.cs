using MessMate.Api.Extensions;
using MessMate.Api.Middlewares;
using MessMate.Api.Services;
using MessMate.API.Extensions;


//using MessMate.Api.Services;
using MessMate.Application;
using MessMate.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSerilogLogging();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<CookieService>();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
