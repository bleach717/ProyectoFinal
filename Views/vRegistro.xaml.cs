using Newtonsoft.Json;
using ProyectoFinal.Models;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ProyectoFinal.Views;

public partial class vRegistro : ContentPage
{
	public vRegistro()
	{
		InitializeComponent();
	}

    private async void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // ========= VALIDACIONES =========

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Error", "El nombre es obligatorio.", "OK");
                return;
            }

            // Validar apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                await DisplayAlert("Error", "El apellido es obligatorio.", "OK");
                return;
            }

            // Validar cédula (exactamente 10 dígitos)
            if (string.IsNullOrWhiteSpace(txtCedula.Text) ||
                txtCedula.Text.Length != 10 ||
                !txtCedula.Text.All(char.IsDigit))
            {
                await DisplayAlert("Error", "La cédula debe tener exactamente 10 dígitos.", "OK");
                return;
            }

            // Validar correo con Regex
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                !Regex.IsMatch(txtCorreo.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Error", "Debes ingresar un correo válido.", "OK");
                return;
            }

            // Validar contraseña (mínimo 15 caracteres)
            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 10)
            {
                await DisplayAlert("Error", "La contraseña debe tener al menos 10 caracteres.", "OK");
                return;
            }

            // Validar confirmar contraseña
            if (txtPassword.Text != txtVerificarPassword.Text)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            // ============= ENVÍO AL MICRO SERVICIO =============
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();

            parametros.Add("nombre", txtNombre.Text);
            parametros.Add("apellido", txtApellido.Text);
            parametros.Add("cedula", txtCedula.Text);
            parametros.Add("correo", txtCorreo.Text);
            parametros.Add("contrasena", txtPassword.Text);
            parametros.Add("rol", "USER");  //  rol por defecto

            byte[] respuestaBytes = cliente.UploadValues("http://127.0.0.1/wsproyecto/restUsuario.php", "POST", parametros);

            string respuestaJson = Encoding.UTF8.GetString(respuestaBytes);

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);
            
            // Si el microservicio devolvió error
            if (data.ContainsKey("error"))
            {
                await DisplayAlert("? Error", data["error"].ToString(), "OK");
                return;
            }

            // Si salió bien
            await DisplayAlert("? Éxito", "Usuario registrado correctamente", "OK");
            await Navigation.PushAsync(new vLogin());

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
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    



    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new vLogin());
    }
}