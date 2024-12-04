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
using AutoMapper;
using Tournament.Core.Dto;

namespace Tournament.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public GamesController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            var games = await _uow.GameRepository.GetAllAsync();
            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest();
            }

            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _mapper.Map(gameDto, game);
            _uow.GameRepository.Update(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
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

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            _uow.GameRepository.Add(game);
            await _uow.CompleteAsync();

            var createdGameDto = _mapper.Map<GameDto>(game);
            return CreatedAtAction("GetGame", new { id = createdGameDto.Id }, createdGameDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _uow.GameRepository.Remove(game);
            await _uow.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await _uow.GameRepository.AnyAsync(id);
        }
    }
}
