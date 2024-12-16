using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Utilities;

namespace Service.Contracts.Interfaces
{
    public interface IGameService
    {
        Task<PagedResponse<GameDto>> GetAllGamesAsync(int pageNumber, int pageSize);
        Task<GameDto> GetGameByIdAsync(int id);
        Task<PagedResponse<GameDto>> GetGamesByTitleAsync(string title, int pageNumber, int pageSize);
        Task<GameDto> CreateGameAsync(GameDto gameDto);
        Task<GameDto> PatchGameAsync(int gameId, JsonPatchDocument<GameDto> patchDocument, ModelStateDictionary modelState);
        Task<PagedResponse<GameDto>> GetGamesAsync(string title, int pageNumber, int pageSize);
        Task UpdateGameAsync(int id, GameDto gameDto);
        Task DeleteGameAsync(int id);
    }
}
