using AutoMapper.QueryableExtensions;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Employees.Endpoints;

public class GetAllEmployees : EndpointWithoutRequest<IQueryable<EmployeeResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
    
    public GetAllEmployees(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
    
    public override void Configure()
    {
        Get("employees");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entities = _webApiService.GetAll<Employee>();
        
        Response = entities.ProjectTo<EmployeeResponse>(_mapper.ConfigurationProvider);
        
        await SendAsync(Response);
    }
}