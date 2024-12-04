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
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentDtos);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);
            return Ok(tournamentDto);
        }

        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound();
            }

            _mapper.Map(tournamentDto, tournamentDetails);
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
                    return StatusCode(500, "An error occurred while updating the tournament.");
                }
            }

            return NoContent();
        }

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournamentDetails = _mapper.Map<TournamentDetails>(tournamentDto);

            try
            {
                _uow.TournamentRepository.Add(tournamentDetails);
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while saving the tournament.");
            }

            var createdTournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);
            return CreatedAtAction(nameof(GetTournamentDetails), new { id = createdTournamentDto.Id }, createdTournamentDto);
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

            try
            {
                _uow.TournamentRepository.Remove(tournamentDetails);
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the tournament.");
            }

            return NoContent();
        }

        private async Task<bool> TournamentDetailsExists(int id)
        {
            return await _uow.TournamentRepository.AnyAsync(id);
        }
    }
}
