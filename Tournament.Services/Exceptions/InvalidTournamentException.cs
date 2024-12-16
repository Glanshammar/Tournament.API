using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services.Exceptions
{
    public class InvalidTournamentException : Exception
    {
        public IEnumerable<string> ValidationErrors { get; }

        public InvalidTournamentException(IEnumerable<string> validationErrors)
            : base("Invalid tournament data.")
        {
            ValidationErrors = validationErrors;
        }
    }
}
