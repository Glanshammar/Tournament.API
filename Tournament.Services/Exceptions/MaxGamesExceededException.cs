using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services.Exceptions
{
    public class MaxGamesExceededException : Exception
    {
        public MaxGamesExceededException()
            : base("A tournament cannot have more than 10 games.") { }
    }
}
