using AutoMapper;
using BillTrack.Core.Contracts.Department;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<DepartmentRequest, Department>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<DepartmentUpdateRequest, Department>();

        CreateMap<Department, DepartmentResponse>();
    }
}