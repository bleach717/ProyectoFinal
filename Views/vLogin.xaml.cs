using ProyectoFinal.Models;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProyectoFinal.Views;

public partial class vLogin : ContentPage
{
	public vLogin()
	{
		InitializeComponent();
	}

    private async void btn_iniciar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // VALIDACIONES
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                await DisplayAlert("Error", "Ingresa tu correo.", "OK");
                return;
            }

            if (!Regex.IsMatch(txtCorreo.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Error", "Correo inválido.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                await DisplayAlert("Error", "Ingresa tu contraseña.", "OK");
                return;
            }

            // === PETICIÓN AL MICRO SERVICIO ===
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();

            parametros.Add("correo", txtCorreo.Text);
            parametros.Add("contrasena", txtPassword.Text);

            try
            {
                byte[] response = cliente.UploadValues("http://127.0.0.1/wsproyecto/restUsuario.php?login=1", "POST", parametros);
                string json = Encoding.UTF8.GetString(response);

                // Deserializar al modelo Usuario
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(json);

                await DisplayAlert("Bienvenido", $"Hola {user.Nombre}", "OK");

                // Enviar variables simples a vPrincipal
                await Navigation.PushAsync(new vPrincipal(user.id_usuario, user.Correo, user.Rol));


            }
            catch (WebException ex)
            {
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();

                    try
                    {
                        var error = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                        if (error.ContainsKey("error"))
                        {
                            await DisplayAlert("? Error", error["error"], "OK");
                            return;
                        }
                    }
                    catch { }

                    await DisplayAlert("Error", "Error inesperado del servidor.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new vRegistro());
    }
}