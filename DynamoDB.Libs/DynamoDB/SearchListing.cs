using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Threading;

namespace DynamoDB.Libs
{
    public class SearchListing : ISearchListing
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private static Table table;
        public static Dictionary<string, AttributeValue> chave;
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;

        public SearchListing(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }
        public async Task<List<Document>> SearchListing_async(Search search)
        {
            int i = 0;
            List<Document> docList = new List<Document>();
            Task<List<Document>> getNextBatch;

            try
            {
                getNextBatch = search.GetNextSetAsync();
                return await getNextBatch;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<QueryResponse> Querying_async<T>(T tabela, string nomeTabela)
        {
            QueryRequest qRequest;
            QueryResponse qResponse;

            try
            {
                qRequest = new QueryRequest(nomeTabela);
                Task<QueryResponse> queryTask = _dynamoClient.QueryAsync(qRequest);
                qResponse = await queryTask;
                return qResponse;
            }

            catch(Exception ex)
            {
                throw ex;
            }

        
        }
    }
}

