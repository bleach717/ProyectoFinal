using ProyectoFinal.Models;
using System;
using System.Collections.ObjectModel;
using static ProyectoFinal.Views.vPrincipal;

namespace ProyectoFinal.Views;

public partial class vTransferirActivo : ContentPage
{
    private Activos_1 _activo;
    private Usuario usuarioSeleccionado;
    private ObservableCollection<Usuario> usuarios = new();

    public vTransferirActivo(Activos_1 activo)
    {
        InitializeComponent();
        _activo = activo;

        lblActivo.Text = activo.Detalle;
        lblPropietarioActual.Text = $"{activo.Nombre} {activo.Apellido}";

        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        try
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync("http://127.0.0.1/wsproyecto/restProyecto.php?table=usuario");

            var lista = System.Text.Json.JsonSerializer.Deserialize<List<Usuario>>(json,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            usuarios.Clear();
            foreach (var u in lista)
                usuarios.Add(u);

            listaUsuarios.ItemsSource = usuarios;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void listaUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        usuarioSeleccionado = e.CurrentSelection.FirstOrDefault() as Usuario;
    }

    private void txtBuscarUsuario_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            listaUsuarios.ItemsSource = usuarios;
        }
        else
        {
            listaUsuarios.ItemsSource = usuarios.Where(u =>
                $"{u.Nombre} {u.Apellido}".Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    private async void BtnConfirmar_Clicked(object sender, EventArgs e)
    {
        if (usuarioSeleccionado == null)
        {
            await DisplayAlert("Atención", "Debe seleccionar un usuario destino.", "OK");
            return;
        }

        try
        {
            using var client = new HttpClient();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("id_activo", _activo.IdActivo.ToString()),
                new KeyValuePair<string,string>("id_usuario_origen", _activo.Propietario.ToString()),
                new KeyValuePair<string,string>("id_usuario_destino", usuarioSeleccionado.id_usuario.ToString())
            });

            var response = await client.PostAsync("http://127.0.0.1/wsproyecto/restProyecto.php?table=transferencia", data);
            response.EnsureSuccessStatusCode();

            await DisplayAlert("Éxito", "Transferencia realizada correctamente.", "OK");

            int id = UserSession.IdUsuario;
            string mail = UserSession.Correo;
            string rol = UserSession.Rol;

            await Navigation.PushAsync(new vPrincipal(id, mail, rol));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
