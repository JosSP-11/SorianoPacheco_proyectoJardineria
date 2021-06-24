using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Input;
using Xamarin.Forms;

namespace JARdineria
{
   public class MiJardin : INotifyPropertyChanged
    {
        public MiJardin()
        {
          
            Abrir();
            AgregarCommand = new Command(AgregarNuevaPlanta);
            NavegarAgregarCommand = new Command(NavegarAgregar);
            FiltrarCommand = new Command(Filtrar);
            VistaPlantaCommand = new Command<NuevaPlanta>(NavegarVistaPlanta);
            EliminarCommand = new Command<NuevaPlanta>(NavegarEliminar);
            Filtrar();
        }

        

        public ObservableCollection <NuevaPlanta> Miplanta{ get; set;}
        public ObservableCollection<NuevaPlanta> Filtrada { get; set; } = new ObservableCollection<NuevaPlanta>();

        public ICommand AgregarCommand { get; set; }
        public ICommand NavegarAgregarCommand { get; set; }
        public ICommand FiltrarCommand { get; set; }
        public ICommand VistaPlantaCommand { get; set; }
        public ICommand EliminarCommand { get; set; }

        private NuevaPlanta nplanta;

        public NuevaPlanta Nplanta
        {
            get { return nplanta; }
            set { nplanta = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nplanta)));
            }
           
        }

        private string nombreP;

        public string NombreP
        {
            get { return nombreP; }
            set { nombreP = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NombreP))); }
        }


        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error))); }
        }

        private void NavegarAgregar()
        {
            Agregar view = new Agregar() { BindingContext = this } ;
            Nplanta = new NuevaPlanta();
            Application.Current.MainPage.Navigation.PushAsync(view);
        }
       public void AgregarNuevaPlanta()
        {
            if (Nplanta!=null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Nplanta.Nombre))
                {
                    Error += "Escriba el nombre de la planta";
                }
                if (Nplanta.FechaPlantado.Date < DateTime.MinValue)
                {
                    Error += "La fecha del plantado no puede ser esta";
                }
                if (string.IsNullOrWhiteSpace(Nplanta.Imagen))
                {
                    Error += "Escriba la direccion de la imagen";
                }
                if (string.IsNullOrWhiteSpace(Nplanta.Nota))
                {
                    Error += "Escriba alguna nota";
                }
                if (Error!="")
                {
                    return;
                }
                Miplanta.Add(Nplanta);
                Filtrar();
                Application.Current.MainPage.Navigation.PopAsync();
                Guardar();
               
                
            }

        }

        private void Filtrar()
        {
            Filtrada.Clear();
          var objetos = Miplanta.Where(x => x.Nombre == NombreP).OrderBy(x=>x.Nombre);

            foreach (var item in objetos)
            {
                Filtrada.Add(item);
            }
        }

        VistaPlanta viewPlanta;
        int posPlantaOriginal;
        private  void NavegarVistaPlanta(NuevaPlanta clon)
        {
            if (viewPlanta==null) 
            {
                viewPlanta = new VistaPlanta() { BindingContext = this };
            }
            NuevaPlanta Clon = new NuevaPlanta() { Nombre = clon.Nombre, FechaPlantado = clon.FechaPlantado.Date, Imagen = clon.Imagen, Nota = clon.Nota };
          
            Nplanta = Clon;
            posPlantaOriginal = Miplanta.IndexOf(clon);
            Application.Current.MainPage.Navigation.PushAsync(viewPlanta);
        }

        private async void NavegarEliminar(NuevaPlanta objEliminar)
        {
           var resultado=await Application.Current.MainPage.DisplayActionSheet("Confirmacion", "No", "Si", "Desea eliminarlo permanentemente?");
            if (resultado=="Si")
            {
                Nplanta = objEliminar;
                Eliminar();
            }
           
        }
        public void Eliminar()
        {
            if (Nplanta!=null)
            {
                if (Miplanta.Remove(Nplanta))
                {
                    Guardar();
                    Filtrar();
                }
            }
        }
        private void Guardar()
        {
           var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var miArchivo = File.CreateText(ruta+"/JARdineria.json");
            string Json = JsonConvert.SerializeObject(Miplanta);
            miArchivo.Write(Json);
            miArchivo.Close();
        }

        private void Abrir()
        {
            try
            {
                var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var miArchivo = File.OpenText(ruta+"/JARdineria.json");
                string Json = miArchivo.ReadToEnd();
                Miplanta = JsonConvert.DeserializeObject<ObservableCollection<NuevaPlanta>>(Json);
                miArchivo.Close();
            }
            catch
            {
                Miplanta = new ObservableCollection<NuevaPlanta>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
