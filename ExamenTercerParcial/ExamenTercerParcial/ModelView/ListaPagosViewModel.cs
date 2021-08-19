using ExamenTercerParcial.Controller;
using ExamenTercerParcial.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ExamenTercerParcial.ModelView
{
    public class ListaPagosViewModel : BaseViewModel
    {
        Crud crud = new Crud();
        Conexion conexion = new Conexion();
        private ObservableCollection<Pagos> _pagos;
        private bool _buttonVer;
        public bool buttonVer
        {
            get { return _buttonVer; }
            set { _buttonVer = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Pagos> Pagos
        {
            get { return _pagos; }
            set { _pagos = value; OnPropertyChanged(); }
        }

        private Pagos _selectedPago;

        public Pagos SelectedPago
        {
            get { return _selectedPago; }
            set { _selectedPago = value; OnPropertyChanged(); }
        }

        public ICommand IrInformacionCommand { private set; get; }
      
        public INavigation Navigation { get; set; }

        public ListaPagosViewModel(INavigation navigation)
        {
            Navigation = navigation;
            IrInformacionCommand = new Command<Type>(async (pageType) => await IrInformacion(pageType));

            Pagos = new ObservableCollection<Pagos>();
         
            mostrar();

        }


        public void mostrar()
        {
            try
            {
                var ppagosList =  crud.SELECT_WHERE();
                foreach (var pagos in ppagosList)
                {
                    Pagos.Add(new Pagos
                    {
                        idPago = pagos.idPago,
                        descripcion = pagos.descripcion,
                        monto = pagos.monto,
                        fecha = pagos.fecha,
                        photo_recibo = pagos.photo_recibo

                    });
                }



            }
            catch (SQLiteException e)
            {


            }
        }

        async Task IrInformacion(Type pageType)
        {
            Pagos = new ObservableCollection<Pagos>();
            if (SelectedPago != null)
            {
                var page = (Page)Activator.CreateInstance(pageType);

                page.BindingContext = new PagosViewModel()
                {
                    Id = SelectedPago.idPago,
                    Descripcion = SelectedPago.descripcion,
                    Monto = SelectedPago.monto,
                    Fecha = SelectedPago.fecha,
                    Foto = SelectedPago.photo_recibo

                };

                await Navigation.PushModalAsync(page);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "Seleccione un Pago de la lista", "ok");
            }
        }

    }
}
