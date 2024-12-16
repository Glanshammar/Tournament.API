using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tournament.Core.Entities;
using Tournament.Core.Dto;

namespace Tournament.Data.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map TournamentDetails to TournamentDto and vice versa
            CreateMap<TournamentDetails, TournamentDto>().ReverseMap();

            // Map Game to GameDto and vice versa
            CreateMap<Game, GameDto>().ReverseMap();
        }
    }
}
