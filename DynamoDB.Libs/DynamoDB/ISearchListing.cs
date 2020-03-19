using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
    public interface ISearchListing
    {
        Task<List<Document>> SearchListing_async(Search search);
        Task<QueryResponse> Querying_async<T> (T tabela, string nomeTabela);
    }
}
