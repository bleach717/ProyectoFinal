using ProyectoFinal.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoFinal.Views
{
    public partial class vPrincipal : ContentPage
    {
        public static class UserSession
        {
            public static int IdUsuario { get; set; }
            public static string Correo { get; set; }
            public static string Rol { get; set; }
        }

        public vPrincipal(int idUsuario, string correo, string rol)
        {
            InitializeComponent();

            // Guardar en sesión global
            UserSession.IdUsuario = idUsuario;
            UserSession.Correo = correo;
            UserSession.Rol = rol;
            BindingContext = new PrincipalViewModel();

          lblDatos.Text = $"ID: {idUsuario} | Usuario: {correo} | Rol: {rol}";
        }

        // Evento que se ejecuta al seleccionar un activo
        private async void listaActivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collection = sender as CollectionView;

            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;

            var activoSeleccionado = e.CurrentSelection[0] as Activos_1;

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
                if (UserSession.Rol == "ADMIN")
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



        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            var activo = button?.CommandParameter as Activos_1;
            if (activo == null)
                return;

            // Navegar a una vista de edición
            await Navigation.PushAsync(new vEditarActivo(activo));
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                // Obtener el botón que envió el evento
                var boton = sender as Button;

                // Obtener el activo enviado con CommandParameter
                var activo = boton?.CommandParameter as Activos_1;

                if (activo == null)
                {
                    await DisplayAlert("Error", "No se pudo obtener el activo a eliminar.", "OK");
                    return;
                }

                // Confirmación
                bool confirmar = await DisplayAlert(
                    "Confirmar eliminación",
                    $"¿Deseas eliminar el activo con código {activo.IdActivo}?",
                    "Sí", "No"
                );

                if (!confirmar)
                    return;

                // URL API (ajústala si tu API usa otro formato)
                string url = $"http://127.0.0.1/wsproyecto/restProyecto.php?table=activo&id={activo.IdActivo}";

                using (HttpClient client = new HttpClient())
                {
                    // Enviar petición DELETE
                    HttpResponseMessage respuesta = await client.DeleteAsync(url);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Activo eliminado correctamente.", "OK");

                        // Actualizar lista (si estás usando binding)
                        if (BindingContext is PrincipalViewModel vm)
                        {
                            vm.Activos.Remove(activo);   // Remueve el ítem de la lista
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el activo.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al eliminar: {ex.Message}", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PrincipalViewModel vm)
                vm.CargarActivos();
        }
    }
}
