using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JARdineria
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Plantas : ContentPage
    {
        public Plantas()
        {
            InitializeComponent();
        }

             
      

        private void Entry_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            ((MiJardin)BindingContext).FiltrarCommand.Execute(null);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaPlanta());
        }

        

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Filtro());
        }
    }
}