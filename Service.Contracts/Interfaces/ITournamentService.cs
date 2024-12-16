using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tournament.Core.Entities;
using Tournament.Core.Utilities;

namespace Service.Contracts.Interfaces
{
    public interface ITournamentService
    {
        Task<PagedResponse<TournamentDto>> GetAllTournamentsAsync(int pageNumber, int pageSize);
        Task<TournamentDto> GetTournamentByIdAsync(int id);
        Task<TournamentDto> CreateTournamentAsync(TournamentDto tournamentDto);
        Task<TournamentDto> PatchTournamentAsync(int id, JsonPatchDocument<TournamentDto> patchDocument, ModelStateDictionary modelState);
        Task UpdateTournamentAsync(int id, TournamentDto tournamentDto);
        Task DeleteTournamentAsync(int id);
    }
}
