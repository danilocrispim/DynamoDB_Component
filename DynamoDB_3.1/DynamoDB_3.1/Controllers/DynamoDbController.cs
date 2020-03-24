using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDB.Libs;
using DynamoDB.Models;
using Amazon.DynamoDBv2.Model;
using System.Net;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DynamoDB_3.Controllers
{
    [Produces("application/json")]
    [Route("api/DynamoDb")]
    public class DynamoDbController : Controller
    {
        private readonly IPutItem _putItem;
        private readonly IGetItem _getItem;
        private readonly IUpdateItem _updateItem;
        private readonly IDeleteItem _deleteItem;
        private readonly IQuery _queryItem;
        private readonly IScan _scanItem;


        public DynamoDbController(IGetItem getItem, IPutItem putItem, IUpdateItem updateItem, IDeleteItem deleteItem, IQuery queryItem, IScan scanItem)
        {
            _getItem = getItem;
            _putItem = putItem;
            _updateItem = updateItem;
            _deleteItem = deleteItem;
            _queryItem = queryItem;
            _scanItem = scanItem;
        }

        #region GET

        [Route("getitems")]
        public async Task<JsonResult> GetItems([FromQuery] int? codigo, string uf)
        {
            Item item = new Item();
            item.Codigo = codigo.Value;
            item.UF = uf;
            var jsonResult = await _getItem.GetItems<Item>(item, Item.nomeTabela);
            return Json(jsonResult);
        }

        #endregion

        #region PUT

        [Route("putitems")]
        public async Task<IActionResult> PutItems([FromQuery] int codigo, string uf, string nome)
        {
            Item item = new Item();
            item.Codigo = codigo;
            item.UF = uf;
            item.Nome = nome;
            var response = await _putItem.PutItems<Item>(item, Item.nomeTabela);

            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                return Ok();
            else
                return BadRequest(response);
        }


        #endregion

        #region UPDATE

        [Route("updateitems")]
        public async Task<IActionResult> UpdateItems([FromQuery] int codigo, string uf, string nome)
        {
            Item item = new Item();
            item.Codigo = codigo;
            item.UF = uf;
            item.Nome = nome;
            var response = await _updateItem.UpdateItems<Item>(item, Item.nomeTabela);
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                return Ok();
            else
                return BadRequest(response);
        }

        #endregion

        #region DELETE

        [Route("deleteitems")]
        public async Task<IActionResult> DeleteItems([FromQuery] int codigo, string uf)
        {
            Item item = new Item();
            item.Codigo = codigo;
            item.UF = uf;
            var response = await _deleteItem.DeleteItems<Item>(item, Item.nomeTabela);
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                return Ok();
            else
                return BadRequest(response);
        }

        #endregion

        #region QUERY
        [Route("queryitems")]
        public async Task<List<string>> QueryItems()
        {
            //Modelo de Query - Codigo (Partition Key sempre deve estar Equals - Sort key (se houver) pode ter outra lógica
            //Este modelo pesquisia pelos campos de Chave
            QueryRequest queryRequest = new QueryRequest()
            {
                TableName = Item.nomeTabela,
                KeyConditionExpression = "Codigo = :v_codigo",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":v_codigo", new AttributeValue { N =  "11" }}}
            };
            var responseQuery = await _queryItem.Querying_async(queryRequest);
            return responseQuery;
         
        }
        #endregion

        #region SCAN

        [Route("scanitems")]
        public async Task<List<string>> ScanItems()
        {

            // Definir condições
            Dictionary<string, Condition> conditions = new Dictionary<string, Condition>();

            // Codigo maior que 11 (greater-than)
            Condition codigoCondition = new Condition();
            codigoCondition.ComparisonOperator = ComparisonOperator.GT;
            codigoCondition.AttributeValueList.Add(new AttributeValue { N = "11" });
            conditions["Codigo"] = codigoCondition;

            //Modelo de Scan - Codigo recupera todos os itens nas condições colocadas
            ScanRequest scanRequest = new ScanRequest()
            {
                //FROM
                TableName = Item.nomeTabela,
                //SELECT
                //ProjectionExpression = "Codigo, UF",
                ////WHERE
                //ScanFilter = conditions


            };
            var responseScan = await _scanItem.Scanning(scanRequest);
            return responseScan;
        }

        #endregion

    }
}
