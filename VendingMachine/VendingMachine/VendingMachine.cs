using System.IO;
namespace VendingMachine
{
    public class VendingMachine : IVendingMachine
    {
        private IDisplayManager displayManager;
        private ITellerMachine atm;
        private IProductCatalogue catalogue;

        private Dictionary<int, int> inventory = new Dictionary<int, int>();  // productId -> amount

        private int availableSlots;

        private int selectedProduct = 0; // 0 -> nothing selected


        public VendingMachine(IDisplayManager displayManager, ITellerMachine atm, IProductCatalogue catalogue, int availableSlots = 3, string stockFile = "")
        {
            this.availableSlots = availableSlots;
            this.displayManager = displayManager;
            this.atm = atm;
            this.catalogue = catalogue;

            // initialize stock
            if (stockFile != "")
            {
                InitializeStock(stockFile);
            }
            else
            {
                inventory[1] = 15;
                inventory[2] = 10;
                inventory[3] = 20;
            }
        }

        public void InsertCoin(decimal coin)
        {
            atm.AddCoin(coin);
            displayManager.Print($"{Translator.Translate("AMOUNT_ENTERED")}: {atm.customerBalance}{atm.CurrencySymbol}");
            if (selectedProduct > 0)
            {
                DispenseProduct(selectedProduct);
            }
        }

        public void ReturnCoins()
        {
            displayManager.Print($"{Translator.Translate("COINS_RETURNED")}: {atm.customerBalance}{atm.CurrencySymbol}");
            atm.ReturnChange();
            displayManager.Print(Translator.Translate("INSERT_COIN"));
        }

        public void DisplayInventory()
        {
            foreach (KeyValuePair<int, int> item in inventory)
            {
                var product = catalogue.GetProduct(item.Key);
                displayManager.Print($"{product.name}: {product.price}{atm.CurrencySymbol} - {item.Value} {Translator.Translate("ITEMS_LEFT")}.");
            }
        }

        public void SelectProduct(int id)
        {
            if (catalogue.Contains(id))
            {
                if (inventory[id] > 0)
                {
                    selectedProduct = id;
                    DispenseProduct(id);
                }
                else
                {
                    displayManager.Print(Translator.Translate("SOLD_OUT"));
                }
            }
            else
            {
                displayManager.Print("Invalid product selected.");
            }
        }

        public void SelectLanguage(string language)
        {
            Translator.SetLanguage(language);
        }

        private void InitializeStock(string filename)
        {
            string path = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "StockFiles", filename });
            var content = FileReaderCSV.Read(path);
            var count = 0;
            foreach (var row in content)
            {
                count++;
                if (count > availableSlots)
                {
                    throw new Exception("Exceeded VendingMachine product slots.");
                }
                if (row.Count() == 2)
                {
                    var id = int.Parse(row[0]);
                    var amount = int.Parse(row[1]);
                    if (catalogue.Contains(id))
                    {
                        inventory[id] = amount;
                    }
                }
                else
                {
                    throw new Exception($"Invalid file format: {path}");
                }
            }
        }

        private void DispenseProduct(int id)
        {
            if (atm.customerBalance >= catalogue.GetProduct(id).price)
            {
                // enough balance
                var product = catalogue.GetProduct(id);
                if (atm.HasExactChange(product.price))
                {
                    // has exact change -> dispensing
                    selectedProduct = 0;

                    inventory[id]--;
                    atm.DeductFromBalance(product.price);
                    displayManager.Print($"{Translator.Translate("DISPENSING")} {product.name}");

                    // change
                    var change = atm.ReturnChange();
                    if (change > 0)
                    {
                        displayManager.Print($"{Translator.Translate("TAKE_CHANGE")}: {change}{atm.CurrencySymbol}");
                    }

                    displayManager.Print(Translator.Translate("THANK_YOU"));
                }
                else
                {
                    // atm does not have exact change
                    displayManager.Print(Translator.Translate("EXACT_CHANGE_ONLY"));
                }
            }
            else
            {
                // insufficient balance
                displayManager.Print($"{Translator.Translate("PRICE")}: {catalogue.GetProduct(id).price}{atm.CurrencySymbol}");
            }

        }

    }
}

