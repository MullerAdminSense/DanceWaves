using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;

namespace DanceWaves.Adapters.Presenters;

/// <summary>
/// Adaptador de apresentação para navegação
/// Implementa a porta INavigationPresenterPort
/// </summary>
public class NavigationPresenterAdapter : INavigationPresenterPort
{
    public async Task<NavigationViewModel> GetNavigationMenuAsync()
    {
        var menuItems = new List<MenuItem>
        {
            new() {
                Id = "administration",
                Label = "Administration",
                Route = "/administration",
                Icon = "⚙️"
            },
            new() {
                Id = "entry-menu",
                Label = "Entry",
                Route = "/entry-menu",
                Icon = "📝"
            },
            new() {
                Id = "registrations",
                Label = "Registrations",
                Route = "/registrations",
                Icon = "✅"
            },
            new() {
                Id = "signup",
                Label = "Sign-up",
                Route = "/signup",
                Icon = "📋"
            }
        };

        return await Task.FromResult(new NavigationViewModel
        {
            MenuItems = menuItems
        });
    }
}
