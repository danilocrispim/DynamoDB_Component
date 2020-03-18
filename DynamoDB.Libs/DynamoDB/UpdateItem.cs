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
using Amazon.Runtime.Internal.Transform;

namespace DynamoDB.Libs
{
    public class UpdateItem : IUpdateItem
    {
        private readonly IGetItem _getItem;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static Table tableUpdate;
        public static Dictionary<string, AttributeValue> chaveGet;
        public static Dictionary<string, AttributeValue> chaveUpdate;        
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;

        public UpdateItem(IGetItem getItem, IAmazonDynamoDB dynamoDbClient)
        {
            _getItem = getItem;
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<UpdateItemResponse> UpdateItems<T>(T tabelaInterna, string nomeTabela)
        {
            tableUpdate = Table.LoadTable(_dynamoDbClient, nomeTabela);
            DynamoDBContext db = new DynamoDBContext(_dynamoDbClient);            
            var respostaGet =  await _getItem.GetItems(tabelaInterna, nomeTabela);
            Document documentResponseGet = Document.FromJson(respostaGet);            
            Document documentRequestUpdate = db.ToDocument(tabelaInterna);
            
            //Variaveis internas
            List<String> keyUpdate = new List<string>();
            Dictionary<string, AttributeValueUpdate> valoresUpdate = new Dictionary<string, AttributeValueUpdate>();

            //Veriricar quais elementos deverão ser modificados para o update
            foreach (var i in documentResponseGet)
            {
                foreach (var j in documentRequestUpdate)
                {
                    if (i.Key == j.Key)
                    {
                        if (i.Value.ToString() != j.Value.ToString())
                        {
                            var valor = j.Value;
                            //valor
                            keyUpdate.Add(j.Key.ToString());
                        }
                    }
                }
            }

            chaveUpdate = tableUpdate.ToAttributeMap(documentRequestUpdate);

            foreach (var item in keyUpdate)
            {
                foreach (var chave in chaveUpdate)
                {
                    if (item == chave.Key)
                    {
                        AttributeValueUpdate valorUpdate = new AttributeValueUpdate(chave.Value, "PUT");
                        valoresUpdate.Add(chave.Key, valorUpdate);
                    }
                      
                }                
            }

            chaveUpdate = Utils.VerificarChaves(chaveUpdate, tableUpdate);
       
            var response = await _dynamoDbClient.UpdateItemAsync(nomeTabela, chaveUpdate, valoresUpdate, cancellationToken);
            db.Dispose();
            return response;

        }
    }
}
