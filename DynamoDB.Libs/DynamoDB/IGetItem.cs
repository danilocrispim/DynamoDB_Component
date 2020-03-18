using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
   public interface IGetItem
    {
        Task<string> GetItems<T>(T tabela, string nomeTabela);
    }
}
