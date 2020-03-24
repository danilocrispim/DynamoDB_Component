using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
    public interface IQuery
    {
        Task<List<Document>> SearchListing_async(Search search);
        Task<List<string>> Querying_async(QueryRequest queryRequest);
    }
}
