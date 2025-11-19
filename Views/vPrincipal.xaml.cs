using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ProyectoFinal.Views;

public partial class vPrincipal : ContentPage, INotifyPropertyChanged
{
    public ObservableCollection<Activo> Activos { get; set; } = new();

    private int _totalActivos = 4;
    public int TotalActivos
    {
        get => _totalActivos;
        set { _totalActivos = value; OnPropertyChanged(nameof(TotalActivos)); }
    }

    private string _valorTotal;
    public string ValorTotal
    {
        get => _valorTotal;
        set { _valorTotal = value; OnPropertyChanged(nameof(ValorTotal)); }
    }

    // Texto de resumen (ej.: "4 activos asignados")
    public string Resumen => $"{TotalActivos} activos asignados";

    public vPrincipal()
    {
        InitializeComponent();

        // Asignamos el BindingContext a la misma instancia para usar las propiedades
        BindingContext = this;

        // Suscribimos la colección para actualizar totales cuando cambie
        Activos.CollectionChanged += Activos_CollectionChanged;

        CargarActivos();
    }

    private void Activos_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // actualizar total cuando la colección cambie
        TotalActivos = Activos.Count;
        // notificar cambio en Resumen también
        OnPropertyChanged(nameof(Resumen));
    }

    private async void btnMenu_Clicked(object sender, EventArgs e)
    {
        if (Shell.Current != null)
            await Shell.Current.GoToAsync("//MenuPrincipal");
    }

    private void CargarActivos()
    {
        // ejemplo de carga (puedes obtenerlo de API)
        Activos.Add(new Activo { Nombre = "MacBook Pro 16\"", Codigo = "LAP-2024-001", Categoria = "Laptop", Estado = "En uso", Icono = "laptop.png" });
        Activos.Add(new Activo { Nombre = "Monitor Dell 27\"", Codigo = "MON-2024-045", Categoria = "Monitor", Estado = "En uso", Icono = "monitor.png" });
        Activos.Add(new Activo { Nombre = "iPhone 15 Pro", Codigo = "PHO-2024-089", Categoria = "Teléfono", Estado = "En uso", Icono = "telefono.png" });
        Activos.Add(new Activo { Nombre = "Teclado Mecánico", Codigo = "ACC-2024-112", Categoria = "Accesorio", Estado = "En uso", Icono = "teclado.png" });

        // Establecer valor total (puedes calcularlo desde los activos si tienen precio)
        ValorTotal = "$8,500";

        // forzamos actualizar Totales (por si la colección ya tenía elementos)
        TotalActivos = Activos.Count;

        // Notificamos Resumen
        OnPropertyChanged(nameof(Resumen));
    }

    // Modelo simple
    public class Activo
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public string Icono { get; set; }
    }
}
