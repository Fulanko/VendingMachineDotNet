using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine;

namespace VendingMachineTests;

[TestClass]
public class ProductCatalogueTest
{
    ProductCatalogue catalogue = new ProductCatalogue("EUR");

    public ProductCatalogueTest() {
        Translator.SetLanguage("EN");
    }

    [TestMethod]
    public void ShouldHaveCorrectProductPrices()
    {
        Assert.AreEqual(catalogue.GetProduct(1).price, 1);
        catalogue = new ProductCatalogue("JPY");
        Assert.AreEqual(catalogue.GetProduct(1).price, 100);
    }

    [TestMethod]
    public void ShouldGetValidProductById()
    {
        Assert.AreEqual(catalogue.GetProduct(1).name, "Cola");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ShouldThrowExceptionOnInvalidProductId()
    {
        var product = catalogue.GetProduct(10);
    }
}
