using Azure;
using Azure.Data.Tables;
using TableStorage;

string connectionString = "AZURE_CONNECTION_STRING_ACCESS_KEY";
string tableName = "Orders";

// Add entity to table storage
AddEntity("O1", "Mobile", 100);
AddEntity("O2", "Laptop", 50);
AddEntity("O3", "Desktop", 70);
AddEntity("O4", "Laptop", 200);

// Read entity
QueryEntity("Laptop");

//Update Entity
UpdateEntity("Desktop", "O3", 300);

//Delete entity
DeleteEntity("Mobile", "01");

void AddEntity(string orderID, string category, int quantity)
{
    TableClient tableClient = new TableClient(connectionString, tableName);

    TableEntity tableEntity = new TableEntity(category, orderID)
    {
        {"quantity",quantity}
    };

    tableClient.AddEntity(tableEntity);
    Console.WriteLine($"Added Entity with order ID {orderID}");
}

void QueryEntity(string category)
{
    TableClient tableClient = new TableClient(connectionString, tableName);

    Pageable<TableEntity> results = tableClient.Query<TableEntity>(entity => entity.PartitionKey == category);

    foreach (TableEntity tableEntity in results)
    {
        Console.WriteLine($"Order Id {tableEntity.RowKey}");
        Console.WriteLine($"Quantity is {tableEntity.GetInt32("quantity")}");

    }
}

void UpdateEntity(string category, string orderID, int quantity)
{
    // Let's first get the entity that we want to update
    TableClient tableClient = new TableClient(connectionString, tableName);
    Order order = tableClient.GetEntity<Order>(category, orderID);
    order.quantity = quantity;

    tableClient.UpdateEntity<Order>(order, ifMatch: ETag.All, TableUpdateMode.Replace);

    Console.WriteLine("Entity updated");
}

void DeleteEntity(string category, string orderID)
{
    TableClient tableClient = new TableClient(connectionString, tableName);
    tableClient.DeleteEntity(category, orderID);
    Console.WriteLine($"Entity with Partition Key {category} and Row Key {orderID} deleted");
}
