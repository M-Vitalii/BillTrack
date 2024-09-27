using AutoMapper;
using BillTrack.Core.Contracts.Project;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectRequest, Project>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<ProjectUpdateRequest, Project>();
            
        CreateMap<Project, ProjectResponse>();
    }
}