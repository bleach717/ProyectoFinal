using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using ProyectoFinal.Models;

namespace ProyectoFinal.Views
{
    public partial class vRegistroActivo : ContentPage
    {
        public vRegistroActivo()
        {
            InitializeComponent();
            CargarTipos();
            UpdateSwitchVisuals(swDisponible.IsToggled);
        }

        private async void CargarTipos()
        {
            try
            {
                WebClient cliente = new WebClient();
                var json = cliente.DownloadString("http://127.0.0.1/wsproyecto/restTipo_activo.php");

                var lista = JsonConvert.DeserializeObject<List<TipoActivo>>(json);

                pickerTipoActivo.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void pickerTipoActivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tipo = (TipoActivo)pickerTipoActivo.SelectedItem;

        }
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (pickerTipoActivo.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Seleccione un tipo de activo.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDetalle.Text))
                {
                    await DisplayAlert("Error", "Ingrese una descripción.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    await DisplayAlert("Error", "Ingrese un nombre.", "OK");
                    return;
                }

                WebClient cliente = new WebClient();
                NameValueCollection parametros = new NameValueCollection();

                var tipo = (TipoActivo)pickerTipoActivo.SelectedItem;

                parametros.Add("id_tipo", tipo.id_tipo.ToString());
                parametros.Add("nombre", txtNombre.Text);
                parametros.Add("detalle", txtDetalle.Text);
                parametros.Add("Marca", txtMarca.Text);
                parametros.Add("Modelo", txtModelo.Text);
                parametros.Add("Serie", txtSerie.Text);
                parametros.Add("Anio", txtAnio.Text);
                parametros.Add("estado", txtEstado.Text);
                parametros.Add("costo", txtCosto.Text);
                parametros.Add("ubicacion", txtUbicacion.Text);
                parametros.Add("disponible", swDisponible.IsToggled ? "1" : "0");
                parametros.Add("fecha_registro", DateTime.Now.ToString("yyyy-MM-dd"));
                parametros.Add("propietario", "1");

                try
                {
                    byte[] response = cliente.UploadValues("http://127.0.0.1/wsproyecto/restActivo.php", "POST", parametros);

                    string json = Encoding.UTF8.GetString(response);

                    await DisplayAlert("Éxito", "Activo registrado correctamente.", "OK");

                    await Navigation.PopAsync();
                }
                catch (WebException ex)
                {
                    using var reader = new StreamReader(ex.Response.GetResponseStream());
                    string result = reader.ReadToEnd();
                    await DisplayAlert("Error", result, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void btnTipo_Clicked(object sender, EventArgs e)
        {

        }

        private void swDisponible_Toggled(object sender, ToggledEventArgs e)
        {
            UpdateSwitchVisuals(e.Value);
        }

        private void UpdateSwitchVisuals(bool isOn)
        {
            if (isOn)
            {
                swDisponible.OnColor = Color.FromHex("#7C3AED");
                swDisponible.ThumbColor = Colors.White;
                lblEstado.Text = "Sí";
                lblEstado.TextColor = Color.FromHex("#7C3AED");
            }
            else
            {
                swDisponible.OnColor = Colors.LightGray;
                swDisponible.ThumbColor = Colors.Gray;
                lblEstado.Text = "No";
                lblEstado.TextColor = Colors.Gray;
            }
        }
    }
}
