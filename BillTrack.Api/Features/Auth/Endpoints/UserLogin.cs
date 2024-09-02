using BillTrack.Core.Contracts.Auth;
using BillTrack.Core.Interfaces.Services;
using FastEndpoints;
using FastEndpoints.Security;

namespace BillTrack.Api.Features.Auth.Endpoints;

public class UserLogin : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IAuthService _authService;

    public UserLogin(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var token = await _authService.GenerateToken(req.Username, req.Password);

        Response = new LoginResponse(token);
        
        await SendAsync(Response);
    }
}