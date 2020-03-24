using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDB.Models
{
    public class Item
    {
        public int Codigo { get; set; }
        public string UF { get; set; }
        public string Nome { get; set; }
        
        public const string nomeTabela = "TB_Estado";
    }
}
