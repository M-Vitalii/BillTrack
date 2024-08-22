using AutoMapper.QueryableExtensions;
using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class GetAllDepartments : EndpointWithoutRequest<IQueryable<DepartmentResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
    
    public GetAllDepartments(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
    
    public override void Configure()
    {
        Get("departments");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entities = _webApiService.GetAll<Department>();
        
        Response = entities.ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider);
        
        await SendAsync(Response);
    }
}