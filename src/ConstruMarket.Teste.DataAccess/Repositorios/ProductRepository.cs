using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConstruMarket.Teste.DataAccess.Contexto;
using ConstruMarket.Teste.Domain.Entidades;
using ConstruMarket.Teste.Domain.Repositorios;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ConstruMarket.Teste.DataAccess.Repositorios
{
    public class ProductRepository : IProductoRepoasitory
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProduct(Produto produto)
        {
            await _context.AddAsync(produto);
        }

        public async Task<Produto> GetProduct(int idProduct)
        {
            return await _context.Produtos.FirstOrDefaultAsync(x => x.ProdutoId.Equals(idProduct));
        }

        public async Task UpdateProduct(Produto produto)
        {
            await Task.Run(() =>
            {
                _context.Update(produto);
            });
        }

        public async Task<List<Produto>> ProductList()
        {
            var arquivoJson = "dataJson.json";
            var directory = new DirectoryInfo( Directory.GetCurrentDirectory());
            var file = directory.GetFiles().FirstOrDefault(f => f.Name.Equals("dataJson.json"));
            if (file is null)
            {
                await GravarArquivo(directory.FullName, arquivoJson);
                
            }
            
            List<Produto> result = null;

            try
            {
                var resultJson = await File.ReadAllTextAsync(Path.Combine(directory.FullName, arquivoJson));
                result = JsonConvert.DeserializeObject<List<ProdutoModel>>(resultJson).Select(x => new Produto
                {
                    ProdutoId = Convert.ToInt32(x.Id),
                    Nome = string.IsNullOrEmpty(x.Nome) ? "NÃO INFROMADO" : x.Nome.Trim(),
                    Marca = string.IsNullOrEmpty(x.Marca) ? "NÃO INFORMADO" : x.Marca.ToString(),
                    CodigoMarca = x.CodigoMarca.ToString(),
                    Preco = x.GetValue(x.Preco)
                })
                    .ToList();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return  result;
        }

        private async Task GravarArquivo(string directoryFullName, string arquivoJson)
        {
            try
            {
                const string url = "http://makeup-api.herokuapp.com/api/v1/products.json";

                var _http = new HttpClient();
                var resultado = await _http.GetAsync(url);

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudoResultado = await resultado.Content.ReadAsStringAsync();

                    await using var stream = new FileStream(Path.Combine(directoryFullName, arquivoJson), FileMode.Create);
                    await using var esctritor = new StreamWriter(stream);
                    await esctritor.WriteLineAsync(conteudoResultado);

                    esctritor.Close();
                    await esctritor.DisposeAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
              
            }

           
        }

        public async Task AddProductRange(List<Produto> produtos)
        {
            await _context.Produtos.AddRangeAsync(produtos);
        }

        public async Task Commit()
        {
           await  _context.SaveChangesAsync();
        }

      
    }

    public class ProdutoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Nome { get; set; }

        [JsonProperty("price")]
        public string Preco { get; set; }


        [JsonProperty("brand")]
        public string Marca { get; set; }

        public Guid CodigoMarca { get; set; } = Guid.NewGuid();


        public decimal GetValue(string valor)
        {

            if (decimal.TryParse(valor, NumberStyles.Currency,new CultureInfo("en-US"),out var result))
            {
                return result;
            }

            return default;
        }
    }
}