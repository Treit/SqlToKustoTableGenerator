# SqlToKustoTableGenerator
Utility to convert existing SQL Server tables to their Kusto (Azure Data Explorer) equivalent.

## Example usage

```
dotnet run Person.Person "Data Source=(local);Initial Catalog=AdventureWorks2019;Trust Server Certificate=True;Integrated Security=true"
```

### Output

```
.create table Person.Person (BusinessEntityID: int, PersonType: string, NameStyle: bool, Title: string, FirstName: string, MiddleName: string, LastName: string, Suffix: string, EmailPromotion: int, AdditionalContactInfo: dynamic, Demographics: dynamic, rowguid: string, ModifiedDate: datetime)
```