using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;

namespace DanceWaves.Application.UseCases;

public class GetNavigationMenuUseCase(INavigationPresenterPort navigationPresenterPort)
{
    private readonly INavigationPresenterPort _navigationPresenterPort = navigationPresenterPort;

    public async Task<NavigationViewModel> ExecuteAsync()
    {
        var navigationMenu = await _navigationPresenterPort.GetNavigationMenuAsync();
        return navigationMenu;
    }
}
