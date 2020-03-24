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
using System.Web.Helpers;

namespace DynamoDB.Libs
{
    public class Query : IQuery
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private static Table table;
        public static Dictionary<string, AttributeValue> chave;
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;

        public Query(IAmazonDynamoDB dynamoClient)
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

        public async Task<List<string>> Querying_async(QueryRequest qRequest)
        {       
            QueryResponse qResponse;
            List<Dictionary<string, AttributeValue>> listaItem = new List<Dictionary<string, AttributeValue>>();
            List<string> listaJson = new List<string>();
            try
            {
                //Identificar tabela
                table = Table.LoadTable(_dynamoClient, qRequest.TableName);
                //Criar contexto
                DynamoDBContext db = new DynamoDBContext(_dynamoClient);
                Task<QueryResponse> queryTask = _dynamoClient.QueryAsync(qRequest);                
                qResponse = await queryTask;
                //Mapear os campos de retorno
                foreach(var item in qResponse.Items)
                {
                    var objDocument = table.FromAttributeMap(item);
                    var objJson = objDocument.ToJson();
                    listaJson.Add(objJson);

                }

                //Converter para Json
                return listaJson;
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

