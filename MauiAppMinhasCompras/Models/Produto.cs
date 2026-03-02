using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Descricao { get; set; }
        public double quantidade { get; set; }
        public double preco { get; set; }

    }
}
