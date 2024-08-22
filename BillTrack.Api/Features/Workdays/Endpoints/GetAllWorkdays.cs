using AutoMapper.QueryableExtensions;
using BillTrack.Core.Contracts.Workday;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Workdays.Endpoints;

public class GetAllWorkdays : EndpointWithoutRequest<IQueryable<WorkdayResponse>>
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
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entities = _webApiService.GetAll<Workday>();
        
        Response = entities.ProjectTo<WorkdayResponse>(_mapper.ConfigurationProvider);
        
        await SendAsync(Response);
    }
}