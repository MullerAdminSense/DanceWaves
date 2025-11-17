using System.Collections.Generic;
using System.Threading.Tasks;

namespace DanceWaves.Application.Ports
{
    public interface INavigationPresenterPort
    {
        Task<NavigationViewModel> GetNavigationMenuAsync();
    }

    public class NavigationViewModel
    {
    public IEnumerable<MenuItem>? MenuItems { get; set; }
    }

    public class MenuItem
    {
    public string? Id { get; set; }
    public string? Label { get; set; }
    public string? Route { get; set; }
    public string? Icon { get; set; }
    }
}
