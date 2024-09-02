using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class GetByIdDepartment : EndpointWithoutRequest<DepartmentResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public GetByIdDepartment(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Get("departments/{id}");
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entity = await _webApiService.GetByIdAsync<Department>(Route<Guid>("id"));
        Response = _mapper.Map<DepartmentResponse>(entity);
        
        await SendAsync(Response);
    }
}