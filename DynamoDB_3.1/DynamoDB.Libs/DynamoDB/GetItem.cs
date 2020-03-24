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
    public class GetItem : IGetItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private static Table table;
        public static Dictionary<string, AttributeValue> chave;
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;

        public GetItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<string> GetItems<T>(T tabelaInterna, string nomeTabela)
        {
            try
            {
                //Identificar tabela
                table = Table.LoadTable(_dynamoClient, nomeTabela);
                //Criar contexto
                DynamoDBContext db = new DynamoDBContext(_dynamoClient);
                //Identificando campos do objeto genérico preenchidos
                Document documentRequest = db.ToDocument(tabelaInterna);
                //Mapeando os campos
                chave = table.ToAttributeMap(documentRequest);
                //Verificar quais campos são necessarios
                chave = Utils.VerificarChaves(chave, table);
                //Criar objeto para requisição na AWS
                var itemRequest = new GetItemRequest(nomeTabela, chave);
                //Realizar o GET
                var response = await _dynamoClient.GetItemAsync(itemRequest, cancellationToken);
                //Receber itens do response
                var responseItems = response.Item;
                //Mapear os campos de retorno
                var document2 = table.FromAttributeMap(response.Item);
                //Garantir o dispose da conexão
                db.Dispose();
                //Converter para Json
                return document2.ToJson();
            }

            catch(Exception ex)
            {
                throw ex;              
            }          
            
        }
    }
}
