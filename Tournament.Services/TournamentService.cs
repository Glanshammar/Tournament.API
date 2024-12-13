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

namespace Tournament.Services
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

        public async Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync(int pageNumber, int pageSize)
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments)
                .Skip((pageNumber - 1) * pageSize)
                .Take(Math.Min(pageSize, 100));
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
    }
}
