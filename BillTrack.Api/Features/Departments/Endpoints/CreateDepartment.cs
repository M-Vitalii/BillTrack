using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class CreateDepartment : Endpoint<DepartmentRequest, DepartmentResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public CreateDepartment(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Post("departments");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(DepartmentRequest r, CancellationToken c)
    {
        var newEntity = await _webApiService.CreateAsync(_mapper.Map<Department>(r));
        Response = _mapper.Map<DepartmentResponse>(newEntity);
        
        await SendAsync(Response);
    }
}