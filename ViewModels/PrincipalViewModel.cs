using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProyectoFinal.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;

public class PrincipalViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly HttpClient client = new HttpClient();
    private const string Url = "http://127.0.0.1/wsproyecto/restProyecto.php?table=activo";

    public ObservableCollection<Activos> Activos { get; set; } = new();

    private string resumen;
    public string Resumen
    {
        get => resumen;
        set { resumen = value; OnPropertyChanged(); }
    }

    private int totalActivos;
    public int TotalActivos
    {
        get => totalActivos;
        set { totalActivos = value; OnPropertyChanged(); }
    }

    private decimal valorTotal;
    public decimal ValorTotal
    {
        get => valorTotal;
        set { valorTotal = value; OnPropertyChanged(); }
    }

    public PrincipalViewModel()
    {
        CargarActivos();
    }

    private async void CargarActivos()
    {
        try
        {
            var json = await client.GetStringAsync(Url);
            System.Diagnostics.Debug.WriteLine("JSON activos: " + json);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            var lista = JsonConvert.DeserializeObject<List<Activos>>(json, settings);

            Activos.Clear();
            foreach (var a in lista)
                Activos.Add(a);

            TotalActivos = Activos.Count;
            ValorTotal = Activos.Sum(a => a.Costo ?? 0);
            Resumen = $"Tienes {TotalActivos} activos asignados";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error cargando activos: " + ex.Message);
        }
    }

    void OnPropertyChanged([CallerMemberName] string propName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
