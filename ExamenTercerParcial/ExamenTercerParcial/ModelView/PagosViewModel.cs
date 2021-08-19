using ExamenTercerParcial.Controller;
using ExamenTercerParcial.Model;
using ExamenTercerParcial.View;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ExamenTercerParcial.ModelView
{
    public class PagosViewModel : BaseViewModel
    {
        Conexion conn = new Conexion();
        Crud crud = new Crud();
        private int id;
        private string _descripcion;
        private double _monto;
        private DateTime _fecha;
        private byte[] _foto;
        private ImageSource camera;
        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged(); }
        }
        public ImageSource Camarabtn
        {
            get { return camera; }
            set { camera = value; OnPropertyChanged();  }
        }
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; OnPropertyChanged(); }
        }
        public double Monto
        {
            get { return _monto; }
            set { _monto = value; OnPropertyChanged(); }
        }
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; OnPropertyChanged(); }
        }
        public byte[] Foto
        {
            get { return _foto; }
            set { _foto = value; OnPropertyChanged(); }
        }
        public ICommand CameraCommand
        {
            get
            {
                return new Command(() => TomarFoto());
            }
        }
        public ICommand listaCommand
        {
            get
            {
                return new Command(() => App.Current.MainPage.Navigation.PushAsync(new ListaPagos()));
            }
        }

        async void TomarFoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", "Camara no disponible", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "AppRun",
                Name = "recibo",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;



            if (file != null)
            {

                Camarabtn = ImageSource.FromStream(() => file.GetStream());

                using (MemoryStream memory = new MemoryStream())
                {

                    Stream stream = file.GetStream();
                    stream.CopyTo(memory);
                    Foto = memory.ToArray();
                }
              
            }
            Visible = true;

        }
        public async void Guardar()
        {
            if (string.IsNullOrEmpty(Descripcion))
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "Campo de descripcion vacio", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(Monto.ToString()))
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "Campo de monto vacio", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(Fecha.ToString()))
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "Campo de fecha vacio", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(Foto.ToString()))
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "Se necesita una foto de recibo para evidencia", "Ok");
                return;
            }
           
            var pagos = new Pagos()
            {
                idPago = Id,
                descripcion = Descripcion,
                monto = Monto,
                fecha = Fecha,
                photo_recibo = Foto
            
            };

            try
            {


                conn.Conn().CreateTable<Pagos>();
                conn.Conn().Insert(pagos);
                conn.Conn().Close();

                await App.Current.MainPage.DisplayAlert("Success", "Pago Guardado", "Ok");
                await App.Current.MainPage.Navigation.PushAsync(new ListaPagos());
                Clear();

            }
            catch (SQLiteException)
            {
           
            }
        }

        public ICommand ClearCommand { private set; get; }
        public ICommand SendEmailCommand { private set; get; }

        public PagosViewModel()
        {

            ClearCommand = new Command(() => Clear());
            Visible = false;
        }

        public ICommand GuardarCommand
        {
            get { return new Command(() => Guardar()); }
        }
        public ICommand DeleteCommand { get { return new Command(() => eliminar()); } }
        public ICommand UpdateCommand { get { return new Command(() => actualizar()); } }
        void Clear()
        {

            Descripcion = string.Empty;
            Monto = 0;
      

        }
        async void eliminar()
        {
            var persona = await crud.getPagosId(Convert.ToInt32(Id));
            bool conf = await App.Current.MainPage.DisplayAlert("Delete", "Eliminar Pago", "Accept", "Cancel");
            if (conf)
            {
                if (persona != null)
                {
                    await crud.Delete(persona);
                    await App.Current.MainPage.DisplayAlert("Delete", "Datos Eliminados", "ok");
                    Clear();
                   await App.Current.MainPage.Navigation.PopModalAsync();

                }
            }

        }
        async void actualizar()
        {

            bool conf = await App.Current.MainPage.DisplayAlert("Update", "Actualizar datos de Empleado", "Accept", "Cancel");
            if (conf)
            {
                if (!string.IsNullOrEmpty(Id.ToString()))
                {
                    Pagos update = new Pagos
                    {
                        idPago = Convert.ToInt32(Id.ToString()),
                        descripcion = Descripcion,
                        monto = Monto,
                        fecha = Fecha,
                        photo_recibo = Foto
                    };
                    await crud.getPagosUpdateId(update);
                    await App.Current.MainPage.DisplayAlert("Update", "Datos de recibo Actualizados", "ok");
                    await App.Current.MainPage.Navigation.PopModalAsync();

                }
            }

        }

    }
}