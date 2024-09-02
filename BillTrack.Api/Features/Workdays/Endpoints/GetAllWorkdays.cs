using BillTrack.Core.Contracts;
using BillTrack.Core.Contracts.Workday;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Workdays.Endpoints;

public class GetAllWorkdays : Endpoint<PaginationRequest, PagedResult<WorkdayResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
    
    public GetAllWorkdays(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
    
    public override void Configure()
    {
        Get("workdays");
    }
    
    public override async Task HandleAsync(PaginationRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllPagedAsync<Workday>(r.Page, r.PageSize);
        
        Response = new PagedResult<WorkdayResponse>
        {
            Items = _mapper.Map<List<WorkdayResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
        
        await SendAsync(Response, cancellation: c);
    }
}