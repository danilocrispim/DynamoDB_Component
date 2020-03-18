using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Libs
{
    public interface IPutItem
    {
        //Task AddNewEntry(int id, string replyDateTime, double price);

        Task<PutItemResponse> PutItems<T>(T tabela, string nomeTabela);
    }
}
