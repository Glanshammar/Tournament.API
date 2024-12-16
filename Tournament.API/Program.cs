using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tournament.API.Extensions;
using Tournament.Core.Repositories;
using Tournament.Data.Data;
using Tournament.Data.Repositories;
using Service.Contracts;
using Service.Contracts.Interfaces;
using Tournament.Presentation.Controllers;
using Tournament.Services.Services;
using Tournament.Services.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Tournament.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TournamentAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentAPIContext") ?? throw new InvalidOperationException("Connection string 'TournamentAPIContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllers(opt =>
            {
                opt.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(GamesController).Assembly)
            .AddApplicationPart(typeof(TournamentDetailsController).Assembly);


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUoW, UoW>();
            builder.Services.AddAutoMapper(typeof(TournamentMappings));
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                };
            });

            // Register managers
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddTransient<ExceptionHandler>();

            var app = builder.Build();
            await app.SeedDataAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    var handler = context.RequestServices.GetRequiredService<ExceptionHandler>();
                    await handler.TryHandleAsync(context, exception, CancellationToken.None);
                });
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseExceptionHandler();
            app.UseStatusCodePages();

            app.Run();
        }
    }
}