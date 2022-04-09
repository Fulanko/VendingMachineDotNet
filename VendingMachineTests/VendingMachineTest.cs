using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VendingMachine;

namespace VendingMachineTests;

[TestClass]
public class VendingMachineTest
{
    private VendingMachine.VendingMachine vendingMachine;
    private Mock<IDisplayManager> mockDisplayManager = new Mock<IDisplayManager>();
    private TellerMachine atm = new TellerMachine("EUR");
    private ProductCatalogue catalogue = new ProductCatalogue("EUR");

    public VendingMachineTest()
    {
        Translator.SetLanguage("EN");
        vendingMachine = new VendingMachine.VendingMachine(mockDisplayManager.Object, atm, catalogue, 3, "");
    }

    [TestMethod]
    public void ShouldReturnCoins()
    {
        vendingMachine.InsertCoin(1);
        Assert.AreEqual(atm.customerBalance, 1);
        vendingMachine.ReturnCoins();
        mockDisplayManager.Verify(displayManager => displayManager.Print("Coins returned: 1€"));
        Assert.AreEqual(atm.customerBalance, 0);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Insert coin"));
    }

    [TestMethod]
    public void InsertCoinShouldDisplayPriceIfProductSelected()
    {
        vendingMachine.SelectProduct(1);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Price: 1€"));
    }

    [TestMethod]
    public void InsertCoinShouldDispenseIfProductSelected()
    {
        vendingMachine.SelectProduct(1);
        vendingMachine.InsertCoin(1);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Dispensing Cola"));
    }

    [TestMethod]
    public void InsertCoinShouldAskForExactChangeIfProductSelected()
    {
        vendingMachine.SelectProduct(2);
        vendingMachine.InsertCoin(1);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Exact change only"));
    }

    [TestMethod]
    public void InsertCoinShouldDisplayChangeIfProductSelected()
    {
        vendingMachine.SelectProduct(2);
        vendingMachine.InsertCoin(1);
        vendingMachine.InsertCoin(0.5m);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Please take your change: 1.0€"));
    }

    [TestMethod]
    public void SelectProductShouldDisplayInvalidProduct()
    {
        vendingMachine.SelectProduct(4);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Invalid product selected."));
    }

    [TestMethod]
    public void SelectProductShouldDisplayOutOfStock()
    {
        vendingMachine = new VendingMachine.VendingMachine(mockDisplayManager.Object, atm, catalogue, 3, "stockFile.csv");
        vendingMachine.SelectProduct(3);
        mockDisplayManager.Verify(displayManager => displayManager.Print("Sold out"));
    }

    [TestMethod]
    public void ShouldSetLanguage()
    {
        vendingMachine.SelectLanguage("EN");
        Assert.AreEqual(Translator.Language, "EN");
        vendingMachine.SelectLanguage("DE");
        Assert.AreEqual(Translator.Language, "DE");
    }

    [TestMethod]
    public void ShouldDisplayInventory()
    {
        vendingMachine.DisplayInventory();
        mockDisplayManager.Verify(displayManager => displayManager.Print("Cola: 1€ - 15 Items left."));
        mockDisplayManager.Verify(displayManager => displayManager.Print("Chips: 0.5€ - 10 Items left."));
        mockDisplayManager.Verify(displayManager => displayManager.Print("Candy: 0.65€ - 20 Items left."));
    }

    [TestMethod]
    public void ShouldInitializeInventory()
    {
        vendingMachine = new VendingMachine.VendingMachine(mockDisplayManager.Object, atm, catalogue, 3, "stockFile.csv");
        vendingMachine.DisplayInventory();
        mockDisplayManager.Verify(displayManager => displayManager.Print("Cola: 1€ - 14 Items left."));
        mockDisplayManager.Verify(displayManager => displayManager.Print("Chips: 0.5€ - 4 Items left."));
        mockDisplayManager.Verify(displayManager => displayManager.Print("Candy: 0.65€ - 0 Items left."));
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ShouldThrowExceptionIfExceedingProductSlots()
    {
        vendingMachine = new VendingMachine.VendingMachine(mockDisplayManager.Object, atm, catalogue, 2, "stockFile.csv");
    }
}
