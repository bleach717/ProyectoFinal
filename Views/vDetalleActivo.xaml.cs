using ProyectoFinal.Models;
using System;

namespace ProyectoFinal.Views;

public partial class vDetalleActivo : ContentPage
{

    private Activos_1 activo;
    private Activos_1 _datosActivo;

    public vDetalleActivo(Activos_1 a)
    {
        InitializeComponent();
        activo = a;
        _datosActivo = a; // ? NECESARIO si aún lo usas
        CargarDatos(a);
    }

    private void CargarDatos(Activos_1 a)
    {
        lblTitulo.Text = a.Detalle;
        lblCodigo.Text = $"Código: {a.IdActivo}";
        lblEstado.Text = a.Estado;

        lblValor.Text = a.Costo.HasValue ? $"${a.Costo.Value}" : "Sin valor";
        lblUbica.Text = a.Ubicacion;
        lblFecha.Text = a.FechaRegistro.ToShortDateString();

        lblPropietario.Text = !string.IsNullOrEmpty(a.Nombre) && !string.IsNullOrEmpty(a.Apellido)
            ? $"{a.Nombre} {a.Apellido}"
            : "-";

        // Bloque de especificaciones
        lblMarca.Text = !string.IsNullOrEmpty(a.Marca) ? a.Marca : "-";
        lblModelo.Text = !string.IsNullOrEmpty(a.Modelo) ? a.Modelo : "-";
        lblSerie.Text = !string.IsNullOrEmpty(a.Serie) ? a.Serie : "-";
        lblAnio.Text = a.Anio.HasValue ? a.Anio.Value.ToString() : "-";
    }

    private async void BtnTransferir_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vTransferirActivo(activo));
    }


}
