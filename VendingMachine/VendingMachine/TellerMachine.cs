using System;
namespace VendingMachine
{
    public class TellerMachine: ITellerMachine
    {

        public decimal customerBalance { get; private set; } = 0;
        public char CurrencySymbol
        {
            get
            {
                switch (this.currency)
                {
                    case "EUR": return '€';
                    case "USD": return '$';
                    case "JPY": return '¥';
                    default: return '\0';
                }
            }
        }

        private string currency = "EUR";
        private SortedDictionary<decimal, int> coinInventory = new SortedDictionary<decimal, int>();

        public TellerMachine(string currency)
        {
            this.currency = currency;

            switch (this.currency)
            {
                case "EUR":
                    coinInventory[0.05m] = 2;
                    coinInventory[0.10m] = 1;
                    coinInventory[0.20m] = 0;
                    coinInventory[0.50m] = 0;
                    coinInventory[1] = 0;
                    coinInventory[2] = 0;
                    break;
                case "USD":
                    coinInventory[0.05m] = 10;
                    coinInventory[0.10m] = 10;
                    coinInventory[0.25m] = 0;
                    coinInventory[1] = 0;
                    coinInventory[2] = 0;
                    break;
                case "JPY":
                    coinInventory[5] = 10;
                    coinInventory[10] = 10;
                    coinInventory[50] = 0;
                    coinInventory[100] = 5;
                    coinInventory[500] = 0;
                    break;
                default:
                    throw new Exception("Unsupported Currency");
            }
        }

        // Add coins to balance if valid, otherwise print error
        public void AddCoin(decimal coin)
        {
            if (IsValidCoin(coin))
            {
                customerBalance += coin;
                coinInventory[coin]++;
                PrintInventory();
            }
            else
            {
                throw new Exception("Rejected invalid coin.");
            }
        }

        public decimal ReturnChange()
        {
            var change = customerBalance;
            while (customerBalance > 0)
            {
                foreach (var coin in coinInventory.Reverse())
                {
                    while (coin.Value > 0 && coin.Key <= customerBalance)
                    {
                        customerBalance -= coin.Key;
                        coinInventory[coin.Key]--;
                    }
                }
            }
            customerBalance = 0;
            PrintInventory();
            return change;
        }

        public void DeductFromBalance(decimal amount)
        {
            if (customerBalance >= amount)
            {
                customerBalance -= amount;
            }
            PrintInventory();
        }

        public bool HasExactChange(decimal price)
        {
            var change = customerBalance - price;
            if (change < 0)
            {
                return false;
            }

            foreach (var coin in coinInventory.Reverse())
            {
                var count = coin.Value;
                while (count > 0 && coin.Key <= change)
                {
                    change -= coin.Key;
                    count--;
                }
            }

            return change == 0;
        }

        private bool IsValidCoin(decimal value)
        {
            return coinInventory.ContainsKey(value);
        }

        public void PrintInventory()
        {
            foreach (var coin in coinInventory)
            {
                Console.WriteLine($"{coin.Key}: {coin.Value}");
            }
        }

    }
}

