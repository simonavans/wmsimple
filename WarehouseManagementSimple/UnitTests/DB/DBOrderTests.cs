using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitTests.DB
{
    public class DBOrderTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBOrder dbOrder;

        public DBOrderTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBOrderTests.db");
            sqliteConnection.Open();

            dbOrder = new(sqliteConnection);
            dbOrder.InitTable(true);
        }

        public void Dispose()
        {
            sqliteConnection.Close();
        }

        [Fact]
        public void GetAll()
        {
            //Arrange
            IList<Order> beforeCreate;
            IList<Order> afterCreate;

            //Act
            beforeCreate = dbOrder.GetAll();
            Create();
            afterCreate = dbOrder.GetAll();

            //Assert
            Assert.Empty(beforeCreate);
            Assert.Single(afterCreate);
        }

        [Fact]
        public void Get()
        {
            //Arrange
            Order orderToGetBeforeCreate = new() { OrderID = 1 };
            Order orderToGetAfterCreate = new() { OrderID = 1 };
            bool resultBeforeCreate;
            bool resultAfterCreate;

            //Act
            resultBeforeCreate = dbOrder.Get(ref orderToGetBeforeCreate);
            Create();
            resultAfterCreate = dbOrder.Get(ref orderToGetAfterCreate);

            //Assert
            Assert.False(resultBeforeCreate);
            Assert.True(resultAfterCreate);

            Assert.Empty(orderToGetBeforeCreate.CustomerName);
            Assert.Empty(orderToGetBeforeCreate.CustomerAddress);
            Assert.Equal("Hello", orderToGetAfterCreate.CustomerName);
            Assert.Equal("World", orderToGetAfterCreate.CustomerAddress);
        }

        [Fact]
        public void Create()
        {
            //Arrange
            bool result;
            Order order = new() {OrderID = -1, CustomerName = "Hello", CustomerAddress = "World" };

            //Act
            result = dbOrder.Create(ref order);

            //Assert
            Assert.True(result);
            Assert.NotEqual(-1, order.OrderID);
        }

        [Fact]
        public void Delete()
        {
            //Arrange
            IList<Order> beforeDelete;
            IList<Order> afterDelete;

            //Act
            Create();
            beforeDelete = dbOrder.GetAll();
            dbOrder.Delete(beforeDelete.First());
            afterDelete = dbOrder.GetAll();

            //Assert
            Assert.Single(beforeDelete);
            Assert.Empty(afterDelete);
        }

        [Fact]
        public void Update()
        {
            //Arrange
            Order beforeUpdate;
            Order afterUpdate;
            string oldCustomerName;
            string oldCustomerAddress;
            const string newCustomerName = "UpdatedName";
            const string newCustomerAddress = "UpdatedAddress";

            //Act
            Create();
            beforeUpdate = dbOrder.GetAll().First();
            oldCustomerName = beforeUpdate.CustomerName;
            oldCustomerAddress = beforeUpdate.CustomerAddress;
            beforeUpdate.CustomerName = newCustomerName;
            beforeUpdate.CustomerAddress = newCustomerAddress;
            dbOrder.Update(beforeUpdate);
            beforeUpdate.CustomerName = oldCustomerName;
            beforeUpdate.CustomerAddress = oldCustomerAddress;
            afterUpdate = dbOrder.GetAll().First();

            //Assert
            Assert.Equal(oldCustomerAddress, beforeUpdate.CustomerAddress);
            Assert.Equal(newCustomerAddress, afterUpdate.CustomerAddress);
            Assert.Equal(oldCustomerName, beforeUpdate.CustomerName);
            Assert.Equal(newCustomerName, afterUpdate.CustomerName);
        }
    }
}
