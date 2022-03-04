using SqlToKustoTableGenerator;
using System.Data;
using System.Text;

if (args.Length == 0)
{
    PrintUsage();
    return;
}

foreach (var arg in args.Select(x => x.ToLowerInvariant()))
{
    switch (arg)
    {
        case "-?":
        case "--?":
        case "/?":
        case "-help":
        case "/help":
        case "--help":
            PrintUsage();
            return;
        default:
            break;
    }
}

var tableName = args[0];

using var conn = args.Length switch
{
    1 => Utils.GetSqlConnection(),
    _ => Utils.GetSqlConnection(args[1])
};

var cmd = conn.CreateCommand();
cmd.CommandText = $"select top 1 * from {tableName}";

using var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

reader.Read();

var sb = new StringBuilder();
sb.Append($".create table {tableName} (");

for (int i = 0; i < reader.FieldCount; i++)
{
    var colname = reader.GetName(i);
    var coltype = reader.GetDataTypeName(i);

    var kustotype = coltype switch
    {
        "bit" => "bool",
        "tinyint" or "smallint" or "int" => "int",
        "bigint" => "long",
        "decimal" or "money" => "decimal",
        "varchar" or "nvarchar" or "char" => "string",
        "smalldatetime" or "datetime" or "date" => "datetime",
        "time" => "timespan",
        "binary" => "dynamic",
        _ => "unknown"
    };

    if (kustotype == "unknown")
    {
        Console.WriteLine($"UNKNOWN TYPE -> {coltype}");
        return;
    }

    if (i > 0)
    {
        sb.Append(", ");
    }

    sb.Append(colname);
    sb.Append(": ");
    sb.Append(kustotype);
}

sb.Append(')');

Console.Write(sb.ToString());

void PrintUsage()
{
    Console.WriteLine();
    Console.WriteLine("SqlToKustoTableGenerator uses the schema from an existing SQL Server table and generates the command");
    Console.WriteLine("to create an equivalent table in Kusto / Azure Data Explorer.");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("SqlToKustoTableGenerator.exe <TableName> [<ConnectionString>]");
    Console.WriteLine();
    Console.WriteLine($"As an alternative to passing a connection string, you may set the following environment variables:");
    Console.WriteLine("SQL_SERVER");
    Console.WriteLine("SQL_USERNAME");
    Console.WriteLine("SQL_PASSWORD");
    Console.WriteLine("SQL_DATABASE");
}