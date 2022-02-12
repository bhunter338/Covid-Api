using AutoMapper;
using Covid_Api.Dtos;
using Covid_Api.Models;

namespace Covid_Api.Profiles
{
    public class DailyDataProfile : Profile
    {
        public DailyDataProfile()
        {
            CreateMap<DailyData, DailyDataReadDto>();

        }
    }
}