using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                //se a variavel _db não for iniciada
                if (_db == null)
                {   // Monta o caminho completo do arquivo do banco de dados SQLite
                    string path = Path.Combine(

                        // Pega a pasta do banco de dados 
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),

                        // Nome do arquivo do banco de dados
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }

                // Retorna a instância já criada ou recém-instanciada
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.ListaProduto());
        }

    }
}