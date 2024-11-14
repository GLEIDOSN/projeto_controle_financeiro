using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Blazor_Finance_Web.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser;
    private DateTime _loginTime;
    private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(30);
    private readonly Timer _sessionCheckTimer;

    public CustomAuthenticationStateProvider()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity()); // Inicializa como anônimo

        // Configura o timer para verificar a sessão a cada minuto
        _sessionCheckTimer = new Timer(TimeSpan.FromMinutes(30));
        _sessionCheckTimer.Elapsed += CheckSessionExpiry;
        _sessionCheckTimer.Start();
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Verifica se a sessão expirou
        if (_currentUser.Identity?.IsAuthenticated == true && DateTime.Now - _loginTime > _sessionTimeout)
        {
            SignOut();
        }

        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public string? GetUserName()
    {
        return _currentUser.Identity?.IsAuthenticated == true
            ? _currentUser.Identity.Name
            : null;
    }

    public async Task SignInAsync(ClaimsPrincipal principal)
    {
        _currentUser = principal;
        _loginTime = DateTime.Now; // Define o momento de login
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        await Task.CompletedTask;
    }

    public void SignOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity()); // Volta ao estado anônimo
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    private void CheckSessionExpiry(object? sender, ElapsedEventArgs e)
    {
        if (_currentUser.Identity?.IsAuthenticated == true && DateTime.Now - _loginTime > _sessionTimeout)
        {
            SignOut();
        }
    }

    public void Dispose()
    {
        _sessionCheckTimer?.Dispose();
    }
}
