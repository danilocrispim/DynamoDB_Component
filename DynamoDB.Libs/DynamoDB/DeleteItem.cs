using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDB.Libs
{
    public class DeleteItem : IDeleteItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private static Table table;
        public static Dictionary<string, AttributeValue> chave;
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;

        public DeleteItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<DeleteItemResponse> DeleteItems<T>(T tabelaInterna, string nomeTabela)
        {
            try
            {
                table = Table.LoadTable(_dynamoClient, nomeTabela);
                //
                DynamoDBContext db = new DynamoDBContext(_dynamoClient);
                //
                Document documentRequest = db.ToDocument(tabelaInterna);
                //Mapeando os campos
                chave = table.ToAttributeMap(documentRequest);
                //Verificar quais campos são necessarios
                chave = Utils.VerificarChaves(chave, table);
                //
                var deleteRequest = new DeleteItemRequest(nomeTabela, chave);
                //
                var deleteResponse = await _dynamoClient.DeleteItemAsync(deleteRequest, cancellationToken);
                //
                db.Dispose();
                //
                return deleteResponse;
            }

            catch(Exception ex)
            {
                throw ex;
            }

        }   
    }
}