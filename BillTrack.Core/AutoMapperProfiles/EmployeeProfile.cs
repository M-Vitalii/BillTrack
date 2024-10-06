using AutoMapper;
using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeRequest, Employee>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<Employee, EmployeeResponse>()
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
            .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project));
        
        CreateMap<EmployeeUpdateRequest, Employee>();
        CreateMap<Employee, EmployeeResponse>();
    }
}