using AutoMapper;
using Service.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public GameService(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync(int pageNumber, int pageSize)
        {
            var games = await _uow.GameRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GameDto>>(games)
                .Skip((pageNumber - 1) * pageSize)
                .Take(Math.Min(pageSize, 100));
        }

        public async Task<GameDto> GetGameByIdAsync(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException("Game not found");
            return _mapper.Map<GameDto>(game);
        }

        public async Task<IEnumerable<GameDto>> GetGamesByTitleAsync(string title, int pageNumber, int pageSize)
        {
            var games = await _uow.GameRepository.GetByTitleAsync(title);
            return _mapper.Map<IEnumerable<GameDto>>(games)
                .Skip((pageNumber - 1) * pageSize)
                .Take(Math.Min(pageSize, 100));
        }

        public async Task<GameDto> CreateGameAsync(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            _uow.GameRepository.Add(game);
            await _uow.CompleteAsync();
            return _mapper.Map<GameDto>(game);
        }

        public async Task UpdateGameAsync(int id, GameDto gameDto)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            _mapper.Map(gameDto, game);
            _uow.GameRepository.Update(game);
            await _uow.CompleteAsync();
        }

        public async Task DeleteGameAsync(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            _uow.GameRepository.Remove(game);
            await _uow.CompleteAsync();
        }
    }

}
