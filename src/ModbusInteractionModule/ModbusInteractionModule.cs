using Microsoft.Practices.Unity;
using NModbus.UI.InteractionModule.Views;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace NModbus.UI.InteractionModule
{
    public class ModbusInteractionModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ModbusInteractionModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ModbusInteractionRegion", typeof(ModbusInteractionView));
            //_container.RegisterTypeForNavigation<ModbusInteractionView>();
        }
    }
}
