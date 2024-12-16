using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.Core.Dto;
using Service.Contracts.Interfaces;
using System.Runtime.InteropServices;
using Tournament.Core.Utilities;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentDetailsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<PagedResponse<TournamentDto>>> GetTournamentDetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var pagedTournaments = await _serviceManager.TournamentService.GetAllTournamentsAsync(pageNumber, pageSize);
            if (pagedTournaments == null || !pagedTournaments.Data.Any())
            {
                return NotFound();
            }
            return Ok(pagedTournaments);
        }


        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournament = await _serviceManager.TournamentService.GetTournamentByIdAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            return Ok(tournament);
        }

        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _serviceManager.TournamentService.UpdateTournamentAsync(id, tournamentDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the tournament.");
            }

            return NoContent();
        }

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            try
            {
                var createdTournament = await _serviceManager.TournamentService.CreateTournamentAsync(tournamentDto);
                return CreatedAtAction(nameof(GetTournamentDetails), new { id = createdTournament.Id }, createdTournament);
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating the tournament.");
            }
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            try
            {
                await _serviceManager.TournamentService.DeleteTournamentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the tournament.");
            }
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId, JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            try
            {
                var updatedTournament = await _serviceManager.TournamentService.PatchTournamentAsync(tournamentId, patchDocument, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(updatedTournament);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the tournament.");
            }
        }
    }
}
