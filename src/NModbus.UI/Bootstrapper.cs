using Microsoft.Practices.Unity;
using NModbus.UI.Common.Core;
using NModbus.UI.Service;
using NModbus.UI.Views;
using Prism.Unity;
using System.Windows;

namespace NModbus.UI
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ModbusMasterManager>(new ContainerControlledLifetimeManager());
            Container.Resolve<ModbusMasterManager>();
            //Container.RegisterType<IApplicationCommands, ApplicationCommands>(
            //    new ContainerControlledLifetimeManager());
        }
    }
}
