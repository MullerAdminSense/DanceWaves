using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;

namespace DanceWaves.Application.UseCases
{
    /// <summary>
    /// Use Case para exibir o menu de navegação
    /// Implementa o núcleo de lógica de negócio para navegação
    /// </summary>
    public class GetNavigationMenuUseCase
    {
        private readonly INavigationPresenterPort _navigationPresenterPort;

        public GetNavigationMenuUseCase(INavigationPresenterPort navigationPresenterPort)
        {
            _navigationPresenterPort = navigationPresenterPort;
        }

        /// <summary>
        /// Executa o caso de uso para obter o menu de navegação
        /// </summary>
        public async Task<NavigationViewModel> ExecuteAsync()
        {
            var navigationMenu = await _navigationPresenterPort.GetNavigationMenuAsync();
            return navigationMenu;
        }
    }
}
