using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync(int pageNumber, int pageSize);
        Task<GameDto> GetGameByIdAsync(int id);
        Task<IEnumerable<GameDto>> GetGamesByTitleAsync(string title, int pageNumber, int pageSize);
        Task<GameDto> CreateGameAsync(GameDto gameDto);
        Task UpdateGameAsync(int id, GameDto gameDto);
        Task DeleteGameAsync(int id);
    }
}
