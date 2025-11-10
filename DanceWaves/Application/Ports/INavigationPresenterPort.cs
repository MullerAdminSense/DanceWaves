using System.Collections.Generic;
using System.Threading.Tasks;

namespace DanceWaves.Application.Ports
{
    /// <summary>
    /// Porta para apresentação de navegação
    /// Define o contrato para exibir o menu de navegação
    /// </summary>
    public interface INavigationPresenterPort
    {
        Task<NavigationViewModel> GetNavigationMenuAsync();
    }

    /// <summary>
    /// ViewModel para dados de navegação
    /// </summary>
    public class NavigationViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
    }

    /// <summary>
    /// Modelo de item de menu
    /// </summary>
    public class MenuItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Route { get; set; }
        public string Icon { get; set; }
    }
}
