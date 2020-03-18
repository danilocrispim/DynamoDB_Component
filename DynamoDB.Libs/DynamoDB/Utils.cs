using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDB.Libs
{
    public class Utils
    {
        public static Dictionary<string, AttributeValue> VerificarChaves(Dictionary<string, AttributeValue> variaveis, Table tabela)
        {
            try
            {
                Dictionary<string, AttributeValue> chavesGet = new Dictionary<string, AttributeValue>();

                foreach (var i in variaveis)
                {
                    foreach (var j in tabela.HashKeys)
                    {
                        if (i.Key == j)
                            chavesGet.Add(i.Key, i.Value);
                    }

                    if (tabela.RangeKeys.Count > 0)
                    {
                        foreach (var k in tabela.RangeKeys)
                        {
                            if (i.Key == k)
                                chavesGet.Add(i.Key, i.Value);
                        }
                    }
                }

                return chavesGet;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
