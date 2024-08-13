using AutoMapper;
using BillTrack.Core.Contracts.Workday;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class WorkdayProfile : Profile
{
    protected WorkdayProfile()
    {
        CreateMap<WorkdayRequest, Workday>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<Workday, WorkdayResponse>();
    }
}