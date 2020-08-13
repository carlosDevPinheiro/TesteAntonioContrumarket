using ConstruMarket.Teste.DataAccess.Repositorios;
using ConstruMarket.Teste.Domain.Entidades;
using ConstruMarket.Teste.Domain.Repositorios;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConstruMarket.Teste.DataAccess.Contexto;

namespace ConstruMarket.Teste.MockApi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var lista = await DownloadDados();

            if (lista is null) return;

            var produtos = await GetListProduct(lista);






            if (!produtos.Any())
            {
                Console.WriteLine("Não foi encontrado produtos para serem adicionados no banco.");
            }

            IProductoRepoasitory repositorio = new ProductRepository(new ApplicationDbContext());
            await repositorio.AddProductRange(produtos);
            await repositorio.Commit();


        }

        private static async Task<List<Produto>> GetListProduct(List<ProdutoModel> lista)
        {
            var products = new List<Produto>();

            try
            {
                await Task.Run(() =>
                {
                    products = lista.Select(x => new Produto
                        {
                            Nome = string.IsNullOrEmpty(x.Nome) ?  "NÃO INFROMADO" : x.Nome.Trim()  ,
                            Marca = string.IsNullOrEmpty(x.Marca) ? "NÃO INFORMADO" : x.CodigoMarca.ToString() ,
                            CodigoMarca =x.CodigoMarca.ToString(),
                            Preco = string.IsNullOrEmpty(x.Preco) ? 150.33m : Convert.ToDecimal(x.Preco)
                        })
                        .ToList();
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }

       

            return await Task.FromResult(products);

        }

        private static async Task<List<ProdutoModel>> DownloadDados()
        {
            List<ProdutoModel> produtoInfo = null;
            try
            {
                const string url = "http://makeup-api.herokuapp.com/api/v1/products.json";

                var _http = new HttpClient();
                var resultado = await _http.GetAsync(url);

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudoResultado = await resultado.Content.ReadAsStringAsync();

                    produtoInfo = JsonConvert.DeserializeObject<List<ProdutoModel>>(conteudoResultado);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" Fim da Execução.");
                }

                return await Task.FromResult(produtoInfo);

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return await Task.FromResult(new List<ProdutoModel>());
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);

                foreach (DictionaryEntry dictionaryEntry in ex.Data)
                {
                    Console.WriteLine($" {dictionaryEntry.Key} : {dictionaryEntry.Value}");
                }
                return await Task.FromResult(new List<ProdutoModel>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult(new List<ProdutoModel>());
            }
        }
    }
}
