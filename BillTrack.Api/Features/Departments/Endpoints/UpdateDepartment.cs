using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class UpdateDepartment : Endpoint<DepartmentUpdateRequest>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public UpdateDepartment(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Put("departments/{id}");
    }
    
    public override async Task HandleAsync(DepartmentUpdateRequest r, CancellationToken c)
    {
        await _webApiService.UpdateAsync(Route<Guid>("id"), _mapper.Map<Department>(r));
        
        await SendResultAsync(TypedResults.Ok());
    }
}