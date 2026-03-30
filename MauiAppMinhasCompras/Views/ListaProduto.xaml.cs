using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

// Esta é a classe da tela que lista os produtos
public partial class ListaProduto : ContentPage
{
    // ObservableCollection é uma coleção que notifica automaticamente a UI quando itens são adicionados ou removidos
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    // Construtor da tela
    public ListaProduto()
    {
        InitializeComponent(); // Inicializa os componentes do XAML

        // Vincula a lista de produtos à ListView no XAML
        lst_produtos.ItemsSource = lista;
    }

    // Método chamado quando a página aparece na tela
    protected async override void OnAppearing()
    {
        base.OnAppearing(); // Sempre bom chamar o base

        try
        {
            // Popula o Picker de categorias com os valores definidos na classe Produto
            pck_Categoria.ItemsSource = Produto.CategoriaLista.Todas;

            // Limpa a lista antes de carregar os dados do banco
            lista.Clear();

            // Busca todos os produtos do banco de dados
            var tmp = await App.Db.GetAll();

            // Adiciona cada produto na ObservableCollection, atualizando automaticamente a ListView
            tmp.ForEach(p => lista.Add(p));
        }
        catch (Exception ex)
        {
            // Mostra erro caso algo falhe
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento do botão "Adicionar" na Toolbar
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        // Navega para a tela de cadastro de novo produto
        Navigation.PushAsync(new NovoProduto());
    }

    // Evento do botão "Somar" na Toolbar
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Soma o total de todos os produtos exibidos na lista
        double soma = lista.Sum(p => p.Total);

        // Mostra um alerta com o valor total
        DisplayAlert("Total dos Produtos", $"O total é {soma:C}", "OK");
    }

    // Evento de remoção de item, acionado pelo MenuItem "Remover" em cada linha da ListView
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            var menuItem = sender as MenuItem;
            var produto = menuItem.BindingContext as Produto; // Recupera o produto correspondente à linha

            // Confirmação do usuário antes de excluir
            bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {produto.Descricao}?", "Sim", "Não");
            if (confirm)
            {
                await App.Db.Delete(produto.Id); // Deleta do banco
                lista.Remove(produto); // Remove da lista exibida
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento chamado ao selecionar um item na ListView
    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Produto p)
        {
            // Abre a tela de edição do produto, passando o produto selecionado
            Navigation.PushAsync(new EditarProduto
            {
                BindingContext = p
            });
        }
    }

    // Evento do SearchBar, chamado quando o texto muda
    private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        AtualizarLista(); // Atualiza a lista filtrando por busca e categoria
    }

    // Evento do Picker de categoria, chamado quando a categoria selecionada muda
    private void pck_Categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizarLista(); // Atualiza a lista filtrando por busca e categoria
    }

    // Método centralizado que atualiza a lista de produtos com base na busca e na categoria
    private async void AtualizarLista()
    {
        try
        {
            string busca = txt_search.Text ?? string.Empty; // Texto digitado na SearchBar
            string categoria = pck_Categoria.SelectedItem?.ToString(); // Categoria selecionada

            lista.Clear(); // Limpa a lista antes de carregar os resultados

            List<Produto> tmp;

            if (!string.IsNullOrEmpty(categoria))
            {
                // Se houver categoria selecionada, filtra por busca e categoria
                tmp = await App.Db.SearchByCategoria(busca, categoria);
            }
            else if (!string.IsNullOrEmpty(busca))
            {
                // Se houver apenas busca, filtra só pela descrição
                tmp = await App.Db.Search(busca);
            }
            else
            {
                // Caso não haja filtro, carrega todos os produtos
                tmp = await App.Db.GetAll();
            }

            // Adiciona os produtos filtrados na lista para exibição
            tmp.ForEach(p => lista.Add(p));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}