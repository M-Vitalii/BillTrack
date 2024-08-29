using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Employees.Endpoints;

public class GetByIdEmployee : EndpointWithoutRequest<EmployeeResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public GetByIdEmployee(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Get("employee/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entity = await _webApiService.GetByIdAsync<Employee>(Route<Guid>("id"));
        Response = _mapper.Map<EmployeeResponse>(entity);
        
        await SendAsync(Response);
    }
}