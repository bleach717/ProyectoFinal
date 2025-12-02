using ProyectoFinal.Models;

namespace ProyectoFinal.Views
{
    public partial class vEditarActivo : ContentPage
    {
        private Activos_1 ActivoActual;

        public vEditarActivo(Activos_1 activo)
        {
            InitializeComponent();
            ActivoActual = activo;

            // Cargar datos en pantalla
            txtDetalle.Text = activo.Detalle;
            txtEstado.Text = activo.Estado;
            txtAnio.Text = activo.Anio?.ToString();
            txtCosto.Text = activo.Costo?.ToString();
            txtMarca.Text = activo.Marca;
            txtModelo.Text = activo.Modelo;
            txtSerie.Text = activo.Serie;
            txtUbicacion.Text = activo.Ubicacion;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            // Actualizar objeto
            ActivoActual.Detalle = txtDetalle.Text;
            ActivoActual.Estado = txtEstado.Text;
            ActivoActual.Anio = int.TryParse(txtAnio.Text, out int anio) ? anio : null;
            ActivoActual.Costo = decimal.TryParse(txtCosto.Text, out decimal costo) ? costo : null;
            ActivoActual.Marca = txtMarca.Text;
            ActivoActual.Modelo = txtModelo.Text;
            ActivoActual.Serie = txtSerie.Text;
            ActivoActual.Ubicacion = txtUbicacion.Text;

            // LLAMADA AL API (PUT)
            using (var client = new HttpClient())
            {
                var url = $"http://127.0.0.1/wsproyecto/restProyecto.php?table=activo&id={ActivoActual.IdActivo}";

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("detalle", ActivoActual.Detalle),
                    new KeyValuePair<string,string>("estado", ActivoActual.Estado),
                    new KeyValuePair<string,string>("anio", ActivoActual.Anio?.ToString() ?? ""),
                    new KeyValuePair<string,string>("costo", ActivoActual.Costo?.ToString() ?? ""),
                    new KeyValuePair<string,string>("marca", ActivoActual.Marca),
                    new KeyValuePair<string,string>("modelo", ActivoActual.Modelo),
                    new KeyValuePair<string,string>("serie", ActivoActual.Serie),
                    new KeyValuePair<string,string>("ubicacion", ActivoActual.Ubicacion),
                });

                await client.PutAsync(url, content);
            }

            await DisplayAlert("OK", "Activo actualizado", "Aceptar");
            await Navigation.PopAsync();
        }
    }
}
