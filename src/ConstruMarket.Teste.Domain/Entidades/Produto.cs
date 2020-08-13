using System;
using System.Collections.Generic;
using System.Text;

namespace ConstruMarket.Teste.Domain.Entidades
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Marca { get; set; }
        public string CodigoMarca { get; set; }
    }
}
