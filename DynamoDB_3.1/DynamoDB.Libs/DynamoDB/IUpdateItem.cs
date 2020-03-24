using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
    public interface IUpdateItem
    {
        Task<UpdateItemResponse> UpdateItems<T>(T tabelaInterna, string nomeTabela);
    }
}
