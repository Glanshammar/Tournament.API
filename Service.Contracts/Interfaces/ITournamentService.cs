using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync(int pageNumber, int pageSize);
        Task<TournamentDto> GetTournamentByIdAsync(int id);
        Task<TournamentDto> CreateTournamentAsync(TournamentDto tournamentDto);
        Task UpdateTournamentAsync(int id, TournamentDto tournamentDto);
        Task DeleteTournamentAsync(int id);
    }
}
