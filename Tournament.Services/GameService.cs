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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tournament.Core.Utilities;

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

        public async Task<PagedResponse<GameDto>> GetAllGamesAsync(int pageNumber, int pageSize)
        {
            if (pageSize > 100) pageSize = 100;
            var games = await _uow.GameRepository.GetAllAsync();
            var totalItems = games.Count();
            var pagedGames = games
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(pagedGames);

            return new PagedResponse<GameDto>(
                gameDtos,
                (int)Math.Ceiling(totalItems / (double)pageSize),
                pageSize,
                pageNumber,
                totalItems
            );
        }

        public async Task<GameDto> GetGameByIdAsync(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException("Game not found");
            return _mapper.Map<GameDto>(game);
        }

        public async Task<PagedResponse<GameDto>> GetGamesByTitleAsync(string title, int pageNumber, int pageSize)
        {
            if (pageSize > 100) pageSize = 100;
            var games = await _uow.GameRepository.GetByTitleAsync(title);
            var totalItems = games.Count();
            var pagedGames = games
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(pagedGames);

            return new PagedResponse<GameDto>(
                gameDtos,
                (int)Math.Ceiling(totalItems / (double)pageSize),
                pageSize,
                pageNumber,
                totalItems
            );
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

        public async Task<PagedResponse<GameDto>> GetGamesAsync(string title, int pageNumber, int pageSize)
        {
            if (pageSize > 100) pageSize = 100;
            IEnumerable<Game> games;

            if (!string.IsNullOrEmpty(title))
            {
                games = await _uow.GameRepository.GetByTitleAsync(title);
            }
            else
            {
                games = await _uow.GameRepository.GetAllAsync();
            }

            var totalItems = games.Count();
            var pagedGames = games
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(pagedGames);

            return new PagedResponse<GameDto>(
                gameDtos,
                (int)Math.Ceiling(totalItems / (double)pageSize),
                pageSize,
                pageNumber,
                totalItems
            );
        }

        public async Task<GameDto> PatchGameAsync(int gameId, JsonPatchDocument<GameDto> patchDocument, ModelStateDictionary modelState)
        {
            if (patchDocument == null)
            {
                throw new ArgumentNullException(nameof(patchDocument), "Patch document cannot be null.");
            }

            var game = await _uow.GameRepository.GetAsync(gameId);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            var gameDto = _mapper.Map<GameDto>(game);
            patchDocument.ApplyTo(gameDto);

            if (!modelState.IsValid)
            {
                throw new ArgumentException("Invalid model state");
            }

            _mapper.Map(gameDto, game);
            await _uow.CompleteAsync();

            return _mapper.Map<GameDto>(game);
        }
    }
}
