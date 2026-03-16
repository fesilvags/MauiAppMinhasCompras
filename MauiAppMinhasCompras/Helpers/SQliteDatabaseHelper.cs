using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        // Objeto que representa a conexão assíncrona com o banco
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método responsável por inserir um novo produto no banco
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Método responsável por atualizar um produto existente
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }

        // Método responsável por excluir um produto pelo Id
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Método que retorna todos os produtos cadastrados
        public Task<List<Produto>> GetAll()
        {
            // Retorna uma lista com todos os registros da tabela Produto
            return _conn.Table<Produto>().ToListAsync();
        }

        // Método para pesquisar produtos pelo nome/descrição
        public Task<List<Produto>> Search(string q)
        {
            // Comando SQL para buscar produtos que contenham o texto digitado
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
