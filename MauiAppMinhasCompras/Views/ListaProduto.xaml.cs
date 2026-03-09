namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    public ListaProduto()
    {
        InitializeComponent();
    }
        
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //assim que o botão com o evento toolbar... for clicado redirecionara o usuario apra a aba NovoProduto
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}