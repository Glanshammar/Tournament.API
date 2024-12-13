using AutoMapper;
using Service.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITournamentService> _tournamentService;
        private readonly Lazy<IGameService> _gameService;

        public ServiceManager(IUoW uow, IMapper mapper)
        {
            _tournamentService = new Lazy<ITournamentService>(() => new TournamentService(uow, mapper));
            _gameService = new Lazy<IGameService>(() => new GameService(uow, mapper));
        }

        public ITournamentService TournamentService => _tournamentService.Value;
        public IGameService GameService => _gameService.Value;
    }
}
