using System.Collections.Generic;
using System.Threading.Tasks;
using ConstruMarket.Teste.Domain.Entidades;

namespace ConstruMarket.Teste.Domain.Repositorios
{
    public interface IProductoRepoasitory
    {
        public Task AddProduct(Produto produto);
        public Task<Produto> GetProduct(int idProduct);
        public Task UpdateProduct(Produto produto);
        public Task<List<Produto>> ProductList();
        public Task AddProductRange(List<Produto> produtos);
        public Task Commit();
    }
}