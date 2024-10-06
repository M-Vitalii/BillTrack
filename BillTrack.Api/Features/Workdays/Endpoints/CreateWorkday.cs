using BillTrack.Core.Contracts.Workday;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Workdays.Endpoints;

public class CreateWorkday : Endpoint<WorkdayRequest, WorkdayResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public CreateWorkday(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Post("workdays");
    }
    
    public override async Task HandleAsync(WorkdayRequest r, CancellationToken c)
    {
        var newEntity = await _webApiService.CreateAsync(_mapper.Map<Workday>(r));
        Response = _mapper.Map<WorkdayResponse>(newEntity);
        
        await SendAsync(Response);
    }
}