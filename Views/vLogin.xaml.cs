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
		await Navigation.PushAsync(new vPrincipal());
    }
}