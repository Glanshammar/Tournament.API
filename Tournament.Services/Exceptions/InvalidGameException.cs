using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services.Exceptions
{
    public class InvalidGameException : Exception
    {
        public IEnumerable<string> ValidationErrors { get; }

        public InvalidGameException(IEnumerable<string> validationErrors)
            : base("Invalid game data.")
        {
            ValidationErrors = validationErrors;
        }
    }
}
