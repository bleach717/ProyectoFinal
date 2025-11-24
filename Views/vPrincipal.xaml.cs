using ProyectoFinal.Models;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoFinal.Views
{
    public partial class vPrincipal : ContentPage
    {
        public vPrincipal()
        {
            InitializeComponent();
            BindingContext = new PrincipalViewModel(); // tu ViewModel que tiene ObservableCollection<Activos>
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

        private void btnMenu_Clicked(object sender, EventArgs e)
        {

        }
    }
}
