using AutoMapper;
using BillTrack.Core.Contracts.Workday;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class WorkdayProfile : Profile
{
    public WorkdayProfile()
    {
        CreateMap<WorkdayRequest, Workday>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<Workday, WorkdayResponse>()
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee));
        
        CreateMap<WorkdayUpdateRequest, Workday>();
        CreateMap<Workday, WorkdayResponse>();
    }
}