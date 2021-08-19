using ExamenTercerParcial.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExamenTercerParcial.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaPagos : ContentPage
    {
        public ListaPagos()
        {
            InitializeComponent();
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new ListaPagosViewModel(Navigation);
        }
    }
}