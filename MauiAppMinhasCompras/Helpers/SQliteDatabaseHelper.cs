using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    // Classe responsável por gerenciar a conexão com o banco SQLite e operações CRUD
    public class SQLiteDatabaseHelper
    {
        // Conexão assíncrona com o banco
        readonly SQLiteAsyncConnection _conn;

        // Construtor: cria a conexão e garante que a tabela Produto exista
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait(); // cria a tabela se não existir
        }

        /// <summary>
        /// Insere um novo produto no banco.
        /// Garante que a categoria seja armazenada em MAIÚSCULO.
        /// </summary>
        public Task<int> Insert(Produto p)
        {
            // Se Categoria estiver preenchida, converte para MAIÚSCULO
            if (!string.IsNullOrEmpty(p.Categoria))
            {
                p.Categoria = p.Categoria.ToUpper();
            }

            return _conn.InsertAsync(p);
        }

        /// <summary>
        /// Atualiza um produto existente no banco.
        /// Inclui agora a Categoria e mantém em MAIÚSCULO.
        /// </summary>
        public Task<int> Update(Produto p)
        {
            if (!string.IsNullOrEmpty(p.Categoria))
            {
                p.Categoria = p.Categoria.ToUpper();
            }

            // SQL de atualização: inclui Categoria
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?";

            // ExecuteAsync retorna a quantidade de linhas afetadas
            return _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id);
        }

        /// <summary>
        /// Remove um produto pelo seu Id
        /// </summary>
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        /// <summary>
        /// Retorna todos os produtos cadastrados
        /// </summary>
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        /// <summary>
        /// Pesquisa produtos filtrando pela descrição
        /// </summary>
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE ?";
            return _conn.QueryAsync<Produto>(sql, "%" + q + "%");
        }

        /// <summary>
        /// Pesquisa produtos filtrando por descrição e categoria ao mesmo tempo.
        /// Garante que a categoria seja consultada em MAIÚSCULO.
        /// </summary>
        public Task<List<Produto>> SearchByCategoria(string busca, string categoria)
        {
            // converte a categoria para MAIÚSCULO para garantir consistência
            if (!string.IsNullOrEmpty(categoria))
            {
                categoria = categoria.ToUpper();
            }

            string sql = "SELECT * FROM Produto WHERE descricao LIKE ? AND Categoria = ?";
            return _conn.QueryAsync<Produto>(sql, "%" + busca + "%", categoria);
        }
    }
}