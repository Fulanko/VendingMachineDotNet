# VendingMachineDotNet
Written in dotNet 6

## Usage
```
Usage: VendingMachine [options]  
Options:  
  -s, --slots=VALUE          the amount of product slots.  
  -c, --currency=VALUE       the accepted currency.  
  -i, --stocks=VALUE         csv file containing the initial stock  
  -h, --help                 show instructions and exit
```  

## Tests
Code coverage report generated using [ReportGenerator](https://github.com/danielpalme/ReportGenerator)
```
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"./coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```
