using ProyectoFinal.Models;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoFinal.Views
{
    public partial class vPrincipal : ContentPage
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }

        public vPrincipal(int idUsuario, string correo, string rol)
        {
            InitializeComponent();
            BindingContext = new PrincipalViewModel(); // tu ViewModel que tiene ObservableCollection<Activos>

            IdUsuario = idUsuario;
            Correo = correo;
            Rol = rol;

            lblDatos.Text = $"ID: {IdUsuario} | Usuario: {Correo} | Rol: {Rol}";
        }

        // Evento que se ejecuta al seleccionar un activo
        private async void listaActivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collection = sender as CollectionView;

            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;

            var activoSeleccionado = e.CurrentSelection[0] as Activos;

            collection.SelectedItem = null; // Limpiar selección

            if (activoSeleccionado != null)
            {
                await Navigation.PushAsync(new vDetalleActivo(activoSeleccionado));
            }
        }

        private async void btnMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Rol == "ADMIN")
                {
                    await Navigation.PushAsync(new vRegistroActivo());
                }
                // Si NO es ADMIN: no hacer nada
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
