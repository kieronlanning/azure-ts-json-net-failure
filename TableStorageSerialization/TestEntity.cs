using Azure;
using Azure.Data.Tables;
using System;

class TestEntity : ITableEntity
{
    public string PartitionKey { get; set; } = default!;
    
    public string RowKey { get; set; } = default!;
    
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }

    public int Version { get; set; }
}
