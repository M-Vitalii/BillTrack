using BillTrack.Core.Contracts.Workday;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Workdays.Endpoints;

public class GetByIdWorkday : EndpointWithoutRequest<WorkdayResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public GetByIdWorkday(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Get("workdays/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entity = await _webApiService.GetByIdAsync<Workday>(Route<Guid>("id"));
        Response = _mapper.Map<WorkdayResponse>(entity);
        
        await SendAsync(Response);
    }
}