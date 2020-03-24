using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
    public class Scan : IScan
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private static Table table;
        public static Dictionary<string, AttributeValue> chave;
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;

        public Scan(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<string>> Scanning(ScanRequest scanRequest)
        {
            List<Dictionary<string, AttributeValue>> listaItem = new List<Dictionary<string, AttributeValue>>();
            List<string> listaJson = new List<string>();
            try
            {
                //Identificar tabela
                table = Table.LoadTable(_dynamoClient, scanRequest.TableName);
                //Criar contexto
                DynamoDBContext db = new DynamoDBContext(_dynamoClient);
                ScanResponse scanResponse = await _dynamoClient.ScanAsync(scanRequest, cancellationToken);

                //Mapear os campos de retorno
                foreach (var item in scanResponse.Items)
                {
                    var objDocument = table.FromAttributeMap(item);
                    var objJson = objDocument.ToJson();
                    listaJson.Add(objJson);
                }
                //Converter para Json
                return listaJson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

     
          
   