using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
    public class PutItem : IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private static Table table;
        public static Dictionary<string, AttributeValue> chave;
        public static CancellationTokenSource cancellationTokenSource;
        public static CancellationToken cancellationToken = default;
        public PutItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }
		//public async Task AddNewEntry(int id, string replyDateTime, double price)
		//{
		//	var queryRequest = RequestBuilder(id, replyDateTime, price);

		//	await PutItemAsync(queryRequest);
		//}

		//private PutItemRequest RequestBuilder(int id, string replyDateTime, double price)
		//{
		//	var item = new Dictionary<string, AttributeValue>
		//	{
		//		{"Id", new AttributeValue {N = id.ToString()} },
		//		{"ReplyDateTime", new AttributeValue {S = replyDateTime.ToString()} },
		//		{"Price", new AttributeValue {N = price.ToString(CultureInfo.InvariantCulture)}}

		//	};

		//	return new PutItemRequest
		//	{
		//		TableName = "TempDynamoDB",
		//		Item = item
		//	};
		//}

		public async Task<PutItemResponse> PutItems<T> (T tabelaInterna, string nomeTabela)
		{
            //Identificar tabela
            table = Table.LoadTable(_dynamoClient, nomeTabela);
            //Criar contexto
            DynamoDBContext db = new DynamoDBContext(_dynamoClient);
            //Identificando campos do objeto genérico preenchidos
            Document documentRequest = db.ToDocument(tabelaInterna);
            //Mapeando os campos
            chave = table.ToAttributeMap(documentRequest);          
            //Realizar o PUT
            PutItemResponse response = await _dynamoClient.PutItemAsync(nomeTabela, chave, cancellationToken);
			//
			db.Dispose();
			//Retornar resposta
			return response;
		}


	}
}
