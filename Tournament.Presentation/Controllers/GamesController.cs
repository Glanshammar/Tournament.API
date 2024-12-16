using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.Core.Dto;
using Service.Contracts;
using Service.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame([FromQuery] string title = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var pagedGames = await _serviceManager.GameService.GetGamesAsync(title, pageNumber, pageSize);
            if (pagedGames == null || !pagedGames.Data.Any())
            {
                return NotFound();
            }
            return Ok(pagedGames);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _serviceManager.GameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _serviceManager.GameService.UpdateGameAsync(id, gameDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the game.");
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            try
            {
                var createdGame = await _serviceManager.GameService.CreateGameAsync(gameDto);
                return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating the game.");
            }
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                await _serviceManager.GameService.DeleteGameAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the game.");
            }
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId, JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            try
            {
                var updatedGame = await _serviceManager.GameService.PatchGameAsync(gameId, patchDocument, ModelState);
                return Ok(updatedGame);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the game.");
            }
        }
    }
}
