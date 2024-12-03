using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IUoW _uow;

        public TournamentDetailsController(IUoW uow)
        {
            _uow = uow;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDetails>>> GetTournamentDetails()
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            return Ok(tournaments);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetails>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            return Ok(tournamentDetails);
        }

        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetails tournamentDetails)
        {
            if (id != tournamentDetails.Id)
            {
                return BadRequest();
            }

            _uow.TournamentRepository.Update(tournamentDetails);

            try
            {
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentDetails tournamentDetails)
        {
            _uow.TournamentRepository.Add(tournamentDetails);
            await _uow.CompleteAsync();

            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDetails);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound();
            }

            _uow.TournamentRepository.Remove(tournamentDetails);
            await _uow.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> TournamentDetailsExists(int id)
        {
            return await _uow.TournamentRepository.AnyAsync(id);
        }
    }
}
