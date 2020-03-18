using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDB.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ReplyDateTime { get; set; }
        public double? Price { get; set; }
        
        public const string nomeTabela = "TempDynamoDB";
    }
}
