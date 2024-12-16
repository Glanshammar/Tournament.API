using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public int GameId { get; }

        public GameNotFoundException(int gameId)
            : base($"Game with ID {gameId} not found.")
        {
            GameId = gameId;
        }

        public GameNotFoundException(int gameId, string message)
            : base(message)
        {
            GameId = gameId;
        }
    }
}
