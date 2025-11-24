using ProyectoFinal.Models;

namespace ProyectoFinal.Views;

public partial class vDetalleActivo : ContentPage
{
    public vDetalleActivo(Activos activo)
    {
        InitializeComponent();
        CargarDatos(activo);


    }

    private void CargarDatos(Activos a)
    {
        lblTitulo.Text = a.Detalle;
        lblCodigo.Text = $"Código: {a.IdActivo}";
        lblEstado.Text = a.Estado;
        lblValor.Text = a.Costo.HasValue ? $"${a.Costo.Value}" : "Sin valor";
        lblUbica.Text = a.Ubicacion;
        lblPropietario.Text = a.Propietario.ToString();
        lblFecha.Text = a.FechaRegistro.ToShortDateString();
    }
}