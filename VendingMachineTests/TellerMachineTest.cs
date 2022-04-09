using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VendingMachine;

namespace VendingMachineTests;

[TestClass]
public class TellerMachineTest
{
    private VendingMachine.TellerMachine atm;

    public TellerMachineTest()
    {
        atm = new VendingMachine.TellerMachine("EUR");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ShouldThrowExceptionIfInvalidCurrency()
    {
        atm = new VendingMachine.TellerMachine("CHF");
    }

    [TestMethod]
    [DataRow("0.05")]
    [DataRow("0.10")]
    [DataRow("0.20")]
    [DataRow("0.50")]
    [DataRow("1")]
    [DataRow("2")]
    public void AddCoinValid(string coin)
    {
        var oldBalance = atm.customerBalance;
        atm.AddCoin(decimal.Parse(coin));
        var newBalance = atm.customerBalance;
        Assert.AreEqual(newBalance, oldBalance + decimal.Parse(coin));
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    [DataRow("-0.05")]
    [DataRow("3")]
    [DataRow("0.02")]
    public void AddCoinInvalid(string coin)
    {
        var oldBalance = atm.customerBalance;
        atm.AddCoin(decimal.Parse(coin));
        var newBalance = atm.customerBalance;
        Assert.AreEqual(newBalance, oldBalance);
    }

    [TestMethod]
    public void ShouldReturnCoins()
    {
        atm.AddCoin(0.5m);
        atm.ReturnChange();
        // TODO: test dictionary content
        Assert.AreEqual(atm.customerBalance, 0);
    }

    [TestMethod]
    public void ShouldDeductFromBalanceIfPossible()
    {
        atm.AddCoin(0.5m);
        atm.DeductFromBalance(1);
        Assert.AreEqual(atm.customerBalance, 0.5m);
        atm.AddCoin(2);
        atm.DeductFromBalance(1);
        Assert.AreEqual(atm.customerBalance, 1.5m);
    }

    [TestMethod]
    public void ShouldReportExactChangeStatus()
    {
        // initial state = 2x 5cent, 1x 10 cent
        atm.AddCoin(1);
        Assert.IsFalse(atm.HasExactChange(0.65m));
        Assert.IsFalse(atm.HasExactChange(0.70m));
        Assert.IsFalse(atm.HasExactChange(0.10m));
        Assert.IsFalse(atm.HasExactChange(2));

        Assert.IsTrue(atm.HasExactChange(0.80m));
        Assert.IsTrue(atm.HasExactChange(1));
        Assert.IsTrue(atm.HasExactChange(0));
    }

    [TestMethod]
    public void ShouldReturnCurrencySymbol()
    {
        Assert.AreEqual(atm.CurrencySymbol, '€');
        atm = new TellerMachine("USD");
        Assert.AreEqual(atm.CurrencySymbol, '$');
        atm = new TellerMachine("JPY");
        Assert.AreEqual(atm.CurrencySymbol, '¥');
    }
}
