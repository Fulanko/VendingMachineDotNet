using Mono.Options;
using VendingMachine;

// VendingMachine entry point
var arguments = Environment.GetCommandLineArgs();

int availableSlots = 3;
string currency = "EUR";
string stockFile = "";
bool showHelp = false;

var options = new OptionSet {
    { "s|slots=", "the amount of product slots.", (int s) => availableSlots = s },
    { "c|currency=", "the accepted currency.", c => currency = c },
    { "i|stocks=", "csv file containing the initial stock", s => stockFile = s },
    { "h|help", "show instructions and exit", h => showHelp = h != null },
};

try
{
    options.Parse(args);
}
catch (OptionException e)
{
    Console.Write("VendingMachine: ");
    Console.WriteLine(e.Message);
    Console.WriteLine("Try `VendingMachine --help' for more information.");
    return;
}

if (showHelp)
{
    Console.WriteLine("Usage: VendingMachine [options]\nOptions:");
    options.WriteOptionDescriptions(Console.Out);
    return;
}

Translator.SetLanguage("EN"); // Default language
var displayManager = new ConsoleDisplayManager();
var atm = new TellerMachine(currency);
var catalogue = new ProductCatalogue(currency);
var vendingMachine = new VendingMachine.VendingMachine(displayManager, atm, catalogue, availableSlots, stockFile);

// user input loop
while (true)
{
    var command = (Console.ReadLine() ?? "").Split(" ");

    switch (command[0])
    {
        case "ENTER":
            try
            {
                vendingMachine.InsertCoin(decimal.Parse(command[1]));
            }
            catch (Exception e)
            {
                displayManager.Print(e.ToString());
            }
            break;
        case "SHOW":
            vendingMachine.DisplayInventory();
            break;
        case "SELECT":
            try
            {
                vendingMachine.SelectProduct(int.Parse(command[1]));
            }
            catch (Exception e)
            {
                displayManager.Print(e.ToString());
            }
            break;
        case "RETURN":
            try
            {
                if (command[1] == "COINS")
                {
                    vendingMachine.ReturnCoins();
                }
                else
                {
                    displayManager.Print("Invalid Command");
                }
            }
            catch (Exception e)
            {
                displayManager.Print(e.ToString());
            }
            break;
        case "LANGUAGE":
            try
            {
                vendingMachine.SelectLanguage(command[1]);
            }
            catch (Exception e)
            {
                displayManager.Print(e.ToString());
            }
            break;
        default:
            displayManager.Print("Invalid Command");
            break;
    }
}