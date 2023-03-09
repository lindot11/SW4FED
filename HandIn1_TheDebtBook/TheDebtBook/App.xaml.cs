using Prism.Ioc;
using System.Windows;
using TheDebtBook.ViewModels;
using TheDebtBook.Views;

namespace TheDebtBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<AddDebtor, AddDebtorViewModel>();
            containerRegistry.RegisterDialog<EditDebtor, EditDebtorViewModel>();
        }
    }
}
