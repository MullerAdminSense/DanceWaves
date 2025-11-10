using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;

namespace DanceWaves.Adapters.Presenters
{
    /// <summary>
    /// Adaptador de apresentação para navegação
    /// Implementa a porta INavigationPresenterPort
    /// Fornece os menus: Entries, Administration, Sign-up, Registrations
    /// </summary>
    public class NavigationPresenterAdapter : INavigationPresenterPort
    {
        public async Task<NavigationViewModel> GetNavigationMenuAsync()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = "entries",
                    Label = "Entries",
                    Route = "/entries",
                    Icon = "📝"
                },
                new MenuItem
                {
                    Id = "administration",
                    Label = "Administration",
                    Route = "/administration",
                    Icon = "⚙️"
                },
                new MenuItem
                {
                    Id = "signup",
                    Label = "Sign-up",
                    Route = "/signup",
                    Icon = "📋"
                },
                new MenuItem
                {
                    Id = "registrations",
                    Label = "Registrations",
                    Route = "/registrations",
                    Icon = "✅"
                }
            };

            return await Task.FromResult(new NavigationViewModel
            {
                MenuItems = menuItems
            });
        }
    }
}
