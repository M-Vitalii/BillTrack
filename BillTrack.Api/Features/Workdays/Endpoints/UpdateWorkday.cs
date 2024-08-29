using BillTrack.Core.Contracts.Workday;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Workdays.Endpoints;

public class UpdateWorkday : Endpoint<WorkdayRequest>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public UpdateWorkday(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Put("workdays/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(WorkdayRequest r, CancellationToken c)
    {
        await _webApiService.UpdateAsync(Route<Guid>("id"), _mapper.Map<Workday>(r));
        
        await SendResultAsync(TypedResults.Ok());
    }
}