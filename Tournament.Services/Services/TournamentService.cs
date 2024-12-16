using Service.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournament.Core.Utilities;

namespace Tournament.Services.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public TournamentService(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResponse<TournamentDto>> GetAllTournamentsAsync(int pageNumber, int pageSize)
        {
            if (pageSize > 100) pageSize = 100;
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            var totalItems = tournaments.Count();
            var pagedTournaments = tournaments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(pagedTournaments);

            return new PagedResponse<TournamentDto>(
                tournamentDtos,
                (int)Math.Ceiling(totalItems / (double)pageSize),
                pageSize,
                pageNumber,
                totalItems
            );
        }




        public async Task<TournamentDto> GetTournamentByIdAsync(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateTournamentAsync(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
            _uow.TournamentRepository.Add(tournament);
            await _uow.CompleteAsync();
            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task UpdateTournamentAsync(int id, TournamentDto tournamentDto)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament not found");

            _mapper.Map(tournamentDto, tournament);
            _uow.TournamentRepository.Update(tournament);
            await _uow.CompleteAsync();
        }

        public async Task DeleteTournamentAsync(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament not found");

            _uow.TournamentRepository.Remove(tournament);
            await _uow.CompleteAsync();
        }

        public async Task<TournamentDto> PatchTournamentAsync(int id, JsonPatchDocument<TournamentDto> patchDocument, ModelStateDictionary modelState)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament not found");

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            patchDocument.ApplyTo(tournamentDto, modelState);

            if (!modelState.IsValid)
            {
                throw new ArgumentException("Invalid model state");
            }

            _mapper.Map(tournamentDto, tournament);

            await _uow.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournament);
        }
    }
}
