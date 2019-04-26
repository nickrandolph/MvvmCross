using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Shell : MvxShell
    {
        public Shell()
        {
            InitializeComponent();
        }
    }
}
