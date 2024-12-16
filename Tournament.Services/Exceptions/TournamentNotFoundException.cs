using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services.Exceptions
{
    public class TournamentNotFoundException : Exception
    {
        public int TournamentId { get; }

        public TournamentNotFoundException(int tournamentId)
            : base($"Tournament with ID {tournamentId} not found.")
        {
            TournamentId = tournamentId;
        }

        public TournamentNotFoundException(int tournamentId, string message)
            : base(message)
        {
            TournamentId = tournamentId;
        }
    }
}
