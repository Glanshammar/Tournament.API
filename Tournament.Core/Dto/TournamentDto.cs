using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.Dto
{
    public class TournamentDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(3);
    }
}
