using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Employees.Endpoints;

public class CreateEmployee : Endpoint<EmployeeRequest, EmployeeResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public CreateEmployee(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Post("employees");
    }
    
    public override async Task HandleAsync(EmployeeRequest r, CancellationToken c)
    {
        var newEntity = await _webApiService.CreateAsync(_mapper.Map<Employee>(r));
        Response = _mapper.Map<EmployeeResponse>(newEntity);
        
        await SendAsync(Response);
    }
}