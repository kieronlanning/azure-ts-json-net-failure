using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System;

const string connectionString = "UseDevelopmentStorage=true";

var pk = $"pk_{Guid.NewGuid()}";
var rk = "rk-source";

var testEntity = new TestEntity
{
    PartitionKey = pk,
    RowKey = rk,
    Version = 166
};

var tableServiceClient = new TableServiceClient(connectionString);
var tableClient = tableServiceClient.GetTableClient("ATableToTestIn");

await tableClient.CreateIfNotExistsAsync();

Console.WriteLine($"Entity created as TestEntity, {nameof(TestEntity.Version)} type is: {testEntity.Version.GetType()}{Environment.NewLine}");

await tableClient.AddEntityAsync(testEntity);

var tableEntity = (await tableClient.GetEntityAsync<TableEntity>(pk, rk)).Value;

Console.WriteLine($"Entity retrieved as TableEntity, {nameof(TestEntity.Version)} type is: {tableEntity[nameof(TestEntity.Version)].GetType()}{Environment.NewLine}");

var tableEntityAsJson = JsonConvert.SerializeObject(tableEntity, Formatting.None);

Console.WriteLine($"TableEntity serialised as:{Environment.NewLine}{Environment.NewLine}{tableEntityAsJson}{Environment.NewLine}");

tableEntity = JsonConvert.DeserializeObject<TableEntity>(tableEntityAsJson);

Console.WriteLine(" >>> ----  Now we're an Int64, but this is JSON.NET or is this TableEntity? ---- <<<");
Console.WriteLine($"Deserialized TableEntity from JSON, {nameof(TestEntity.Version)} type is: {tableEntity[nameof(TestEntity.Version)].GetType()}{Environment.NewLine}");

rk = "rk-destiantion";

tableEntity.RowKey = rk;
tableEntity.ETag = ETag.All;

Console.WriteLine($"Writing back to table with updated row key.{Environment.NewLine}");

await tableClient.AddEntityAsync(tableEntity);

Console.WriteLine($"Getting the updated entity as TestEntity will throw...{Environment.NewLine}");

testEntity = (await tableClient.GetEntityAsync<TestEntity>(pk, rk)).Value;
