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
       
    
        public DynamoDbController(IGetItem getItem, IPutItem putItem, IUpdateItem updateItem, IDeleteItem deleteItem)
        {
            _getItem = getItem;
            _putItem = putItem;
            _updateItem = updateItem;
            _deleteItem = deleteItem;
        }

        #region GET

        [Route("getitems")]
        public async Task<JsonResult> GetItems([FromQuery] int? id, string replydatetime)
        {
            Item item = new Item();
            item.Id = id.Value;
            item.ReplyDateTime = replydatetime;
            var jsonResult = await _getItem.GetItems<Item>(item, Item.nomeTabela);
            return Json(jsonResult);
        }
       
        #endregion

        #region PUT

        [Route("putitems")]
        public async Task<IActionResult> PutItems([FromQuery] int id, string replydatetime, double? price)
        {
            Item item = new Item();
            item.Id = id;
            item.Price = price;
            item.ReplyDateTime = replydatetime;
            var response = await _putItem.PutItems<Item>(item, Item.nomeTabela);

            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                return Ok();
            else
                return BadRequest(response);                  
        }


        #endregion

        #region UPDATE

        [Route("updateitems")]
        public async Task<IActionResult> UpdateItems([FromQuery] int id, string replydatetime, double price)
        {
            Item item = new Item();
            item.Id = id;
            item.ReplyDateTime = replydatetime;
            item.Price = price;
            var response = await _updateItem.UpdateItems<Item>(item, Item.nomeTabela);
             if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                return Ok();
            else
                return BadRequest(response);
        }

        #endregion

        #region DELETE

        [Route("deleteitems")]
        public async Task<IActionResult> DeleteItems([FromQuery] int id, string replydatetime)
        {
            Item item = new Item();
            item.Id = id;
            item.ReplyDateTime = replydatetime;
            var response = await _deleteItem.DeleteItems<Item>(item, Item.nomeTabela);
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                return Ok();
            else
                return BadRequest(response);
        }

        #endregion



    }
}
