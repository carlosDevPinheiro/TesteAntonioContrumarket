using ConstruMarket.Teste.Domain.Entidades;
using ConstruMarket.Teste.Domain.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstruMarket.Teste.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProdutoController: ControllerBase
    {

        public IProductoRepoasitory ProductoRepoasitory { get; set; }

        public ProdutoController(IProductoRepoasitory productoRepoasitory)
        {
            ProductoRepoasitory = productoRepoasitory;
        }


        [HttpGet]
        public async Task<IEnumerable<Produto>> Get()
        {

            var produtcs = await ProductoRepoasitory.ProductList();


           
            return produtcs;
        }
    }
}
