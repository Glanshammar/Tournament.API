using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case GameNotFoundException gameNotFound:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Game Not Found";
                    problemDetails.Detail = $"Game with ID {gameNotFound.GameId} was not found.";
                    break;
                case TournamentNotFoundException tournamentNotFound:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Tournament Not Found";
                    problemDetails.Detail = $"Tournament with ID {tournamentNotFound.TournamentId} was not found.";
                    break;
                case InvalidGameException invalidGame:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Invalid Game Data";
                    problemDetails.Detail = string.Join(", ", invalidGame.ValidationErrors);
                    break;
                case InvalidTournamentException invalidTournament:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Invalid Tournament Data";
                    problemDetails.Detail = string.Join(", ", invalidTournament.ValidationErrors);
                    break;
            }

            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = (int)problemDetails.Status;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

    }
}
