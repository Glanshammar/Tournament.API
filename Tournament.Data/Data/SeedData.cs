using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(TournamentAPIContext context)
        {
            if (!context.TournamentDetails.Any())
            {
                var tournaments = new List<TournamentDetails>
                {
                    new TournamentDetails { Title = "Summer Cup 2024", StartDate = new DateTime(2024, 6, 1) },
                    new TournamentDetails { Title = "Winter Championship", StartDate = new DateTime(2024, 12, 1) }
                };

                await context.TournamentDetails.AddRangeAsync(tournaments);
                await context.SaveChangesAsync();

                var games = new List<Game>
                {
                    new Game { Title = "Match 1", Time = new DateTime(2024, 6, 1, 14, 0, 0), TournamentId = tournaments[0].Id },
                    new Game { Title = "Match 2", Time = new DateTime(2024, 6, 1, 16, 0, 0), TournamentId = tournaments[0].Id },
                    new Game { Title = "Match 1", Time = new DateTime(2024, 12, 1, 13, 0, 0), TournamentId = tournaments[1].Id },
                    new Game { Title = "Match 2", Time = new DateTime(2024, 12, 1, 15, 0, 0), TournamentId = tournaments[1].Id }
                };

                await context.Games.AddRangeAsync(games);
                await context.SaveChangesAsync();
            }
        }
    }
}
