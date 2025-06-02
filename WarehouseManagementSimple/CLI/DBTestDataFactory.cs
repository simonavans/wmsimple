using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class DBTestDataFactory
    {
        private readonly SqliteConnection _connection;
        public DBTestDataFactory(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTables(bool dropTable = false)
        {
            new DBStUnitContent(_connection).InitTable(dropTable);
            new DBOrderContent(_connection).InitTable(dropTable);
            new DBOrder(_connection).InitTable(dropTable);
            new DBArticle(_connection).InitTable(dropTable);
            new DBStUnit(_connection).InitTable(dropTable);
            new DBLocation(_connection).InitTable(dropTable);
        }

        public void LoadAllData()
        {
            LoadArticleData();
            LoadLocationData();
            LoadStUnitData();
            LoadOrderData();
        }

        public void PurgeData()
        {
            new DBStUnitContent(_connection).Purge();
            new DBOrderContent(_connection).Purge();
            new DBOrder(_connection).Purge();
            new DBArticle(_connection).Purge();
            new DBStUnit(_connection).Purge();
            new DBLocation(_connection).Purge();
        }

        public void LoadArticleData()
        {
            DBArticle dbArticle = new DBArticle(_connection);

            var article1 = new Article { Partno = "LAPTOP", Revision = "", DimensionsLength = 0.50m, DimensionsWidth = 0.31m, DimensionsHeight = 0.06m };
            var article2 = new Article { Partno = "LAPTOP", Revision = "REFURBISHED", DimensionsLength = 0.50m, DimensionsWidth = 0.31m, DimensionsHeight = 0.06m };
            var article3 = new Article { Partno = "TABLET", Revision = "", DimensionsLength = 0.21m, DimensionsWidth = 0.13m, DimensionsHeight = 0.06m };
            var article4 = new Article { Partno = "PHONE", Revision = "", DimensionsLength = 0.17m, DimensionsWidth = 0.09m, DimensionsHeight = 0.03m };
            var article5 = new Article { Partno = "FRIDGE", Revision = "", DimensionsLength = 0.60m, DimensionsWidth = 0.65m, DimensionsHeight = 2.01m };
            var article6 = new Article { Partno = "TV", Revision = "", DimensionsLength = 1.45m, DimensionsWidth = 0.90m, DimensionsHeight = 0.31m };
            var article7 = new Article { Partno = "GAMECONSOLE", Revision = "", DimensionsLength = 0.43m, DimensionsWidth = 0.35m, DimensionsHeight = 0.17m };
            var article8 = new Article { Partno = "WASHINGMACHINE", Revision = "", DimensionsLength = 0.59m, DimensionsWidth = 0.65m, DimensionsHeight = 0.85m };
            var article9 = new Article { Partno = "HEADPHONES", Revision = "", DimensionsLength = 0.19m, DimensionsWidth = 0.05m, DimensionsHeight = 0.16m };
            var article10 = new Article { Partno = "CAMERA", Revision = "", DimensionsLength = 0.12m, DimensionsWidth = 0.07m, DimensionsHeight = 0.10m };

            dbArticle.Create(ref article1);
            dbArticle.Create(ref article2);
            dbArticle.Create(ref article3);
            dbArticle.Create(ref article4);
            dbArticle.Create(ref article5);
            dbArticle.Create(ref article6);
            dbArticle.Create(ref article7);
            dbArticle.Create(ref article8);
            dbArticle.Create(ref article9);
            dbArticle.Create(ref article10);

        }

        public void LoadLocationData()
        {
            DBLocation dbLocation = new DBLocation(_connection);

            var location2 = new Location { Mha = "INBOUND02", Rack = "", HorCoor = "", VerCoor = "" };
            var location3 = new Location { Mha = "INBOUND03", Rack = "", HorCoor = "", VerCoor = "" };
            var location4 = new Location { Mha = "INBOUND04", Rack = "", HorCoor = "", VerCoor = "" };
            var location5 = new Location { Mha = "INBOUND05", Rack = "", HorCoor = "", VerCoor = "" };

            var location6  = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "0", VerCoor = "0" };
            var location7  = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "1", VerCoor = "0" };
            var location8  = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "2", VerCoor = "0" };
            var location9  = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "3", VerCoor = "0" };
            var location10 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "4", VerCoor = "0" };
            var location11 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "5", VerCoor = "0" };
            var location12 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "0", VerCoor = "1" };
            var location13 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "1", VerCoor = "1" };
            var location14 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "2", VerCoor = "1" };
            var location15 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "3", VerCoor = "1" };
            var location16 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "4", VerCoor = "1" };
            var location17 = new Location { Mha = "BUFFER", Rack = "1", HorCoor = "5", VerCoor = "1" };
            var location18 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "0", VerCoor = "0" };
            var location19 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "1", VerCoor = "0" };
            var location20 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "2", VerCoor = "0" };
            var location21 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "3", VerCoor = "0" };
            var location22 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "4", VerCoor = "0" };
            var location23 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "5", VerCoor = "0" };
            var location24 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "0", VerCoor = "1" };
            var location25 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "1", VerCoor = "1" };
            var location26 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "2", VerCoor = "1" };
            var location27 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "3", VerCoor = "1" };
            var location28 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "4", VerCoor = "1" };
            var location29 = new Location { Mha = "BUFFER", Rack = "2", HorCoor = "5", VerCoor = "1" };

            var location30 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "0", VerCoor = "0" };
            var location31 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "1", VerCoor = "0" };
            var location32 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "2", VerCoor = "0" };
            var location33 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "3", VerCoor = "0" };
            var location34 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "4", VerCoor = "0" };
            var location35 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "5", VerCoor = "0" };
            var location36 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "0", VerCoor = "1" };
            var location37 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "1", VerCoor = "1" };
            var location38 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "2", VerCoor = "1" };
            var location39 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "3", VerCoor = "1" };
            var location40 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "4", VerCoor = "1" };
            var location41 = new Location { Mha = "PICKING", Rack = "1", HorCoor = "5", VerCoor = "1" };
            var location42 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "0", VerCoor = "0" };
            var location43 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "1", VerCoor = "0" };
            var location44 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "2", VerCoor = "0" };
            var location45 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "3", VerCoor = "0" };
            var location46 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "4", VerCoor = "0" };
            var location47 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "5", VerCoor = "0" };
            var location48 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "0", VerCoor = "1" };
            var location49 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "1", VerCoor = "1" };
            var location50 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "2", VerCoor = "1" };
            var location51 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "3", VerCoor = "1" };
            var location52 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "4", VerCoor = "1" };
            var location53 = new Location { Mha = "PICKING", Rack = "2", HorCoor = "5", VerCoor = "1" };

            var location54 = new Location { Mha = "PICKINGAREA", Rack = "", HorCoor = "", VerCoor = "" };

            var location55 = new Location { Mha = "OUTBOUND01", Rack = "", HorCoor = "", VerCoor = "" };
            var location56 = new Location { Mha = "OUTBOUND02", Rack = "", HorCoor = "", VerCoor = "" };
            var location57 = new Location { Mha = "OUTBOUND03", Rack = "", HorCoor = "", VerCoor = "" };
            var location58 = new Location { Mha = "OUTBOUND04", Rack = "", HorCoor = "", VerCoor = "" };
            var location59 = new Location { Mha = "OUTBOUND05", Rack = "", HorCoor = "", VerCoor = "" };

            var location60 = new Location { Mha = "MOBILEUNIT", Rack = "", HorCoor = "", VerCoor = "" };

            // Calls with ref
            dbLocation.Create(ref location2);
            dbLocation.Create(ref location3);
            dbLocation.Create(ref location4);
            dbLocation.Create(ref location5);
            dbLocation.Create(ref location6);
            dbLocation.Create(ref location7);
            dbLocation.Create(ref location8);
            dbLocation.Create(ref location9);
            dbLocation.Create(ref location10);
            dbLocation.Create(ref location11);
            dbLocation.Create(ref location12);
            dbLocation.Create(ref location13);
            dbLocation.Create(ref location14);
            dbLocation.Create(ref location15);
            dbLocation.Create(ref location16);
            dbLocation.Create(ref location17);
            dbLocation.Create(ref location18);
            dbLocation.Create(ref location19);
            dbLocation.Create(ref location20);
            dbLocation.Create(ref location21);
            dbLocation.Create(ref location22);
            dbLocation.Create(ref location23);
            dbLocation.Create(ref location24);
            dbLocation.Create(ref location25);
            dbLocation.Create(ref location26);
            dbLocation.Create(ref location27);
            dbLocation.Create(ref location28);
            dbLocation.Create(ref location29);
            dbLocation.Create(ref location30);
            dbLocation.Create(ref location31);
            dbLocation.Create(ref location32);
            dbLocation.Create(ref location33);
            dbLocation.Create(ref location34);
            dbLocation.Create(ref location35);
            dbLocation.Create(ref location36);
            dbLocation.Create(ref location37);
            dbLocation.Create(ref location38);
            dbLocation.Create(ref location39);
            dbLocation.Create(ref location40);
            dbLocation.Create(ref location41);
            dbLocation.Create(ref location42);
            dbLocation.Create(ref location43);
            dbLocation.Create(ref location44);
            dbLocation.Create(ref location45);
            dbLocation.Create(ref location46);
            dbLocation.Create(ref location47);
            dbLocation.Create(ref location48);
            dbLocation.Create(ref location49);
            dbLocation.Create(ref location50);
            dbLocation.Create(ref location51);
            dbLocation.Create(ref location52);
            dbLocation.Create(ref location53);
            dbLocation.Create(ref location54);
            dbLocation.Create(ref location55);
            dbLocation.Create(ref location56);
            dbLocation.Create(ref location57);
            dbLocation.Create(ref location58);
            dbLocation.Create(ref location59);
            dbLocation.Create(ref location60);
        }

        public void LoadOrderData()
        {
            Order order;
            Article article;
            OrderContent orderContent;
            DBOrder dbOrder               = new DBOrder(_connection);
            DBOrderContent dbOrderContent = new DBOrderContent(_connection);
            DBArticle dbArticle           = new DBArticle(_connection);

            // ORDER 1
            order = new Order() { CustomerName = "James Blow", CustomerAddress = "Rotterdam" };
            dbOrder.Create(ref order);

            article = new Article() { Partno = "FRIDGE", Revision = "" };
            dbArticle.Get(ref article);

            orderContent = new OrderContent() { FK_OrderID = order.OrderID, FK_ArticleID = article.ArticleID, Quantity = 1 };
            dbOrderContent.Create(ref orderContent);

            // ORDER 2
            order = new Order() { CustomerName = "Dirk Oranje", CustomerAddress = "Amsterdam" };
            dbOrder.Create(ref order);

            article = new Article() { Partno = "LAPTOP", Revision = "REFURBISHED" };
            dbArticle.Get(ref article);

            orderContent = new OrderContent() { FK_OrderID = order.OrderID, FK_ArticleID = article.ArticleID, Quantity = 10 };
            dbOrderContent.Create(ref orderContent);

            article = new Article() { Partno = "PHONE", Revision = "" };
            dbArticle.Get(ref article);

            orderContent = new OrderContent() { FK_OrderID = order.OrderID, FK_ArticleID = article.ArticleID, Quantity = 20 };
            dbOrderContent.Create(ref orderContent);
        }

        public void LoadStUnitData()
        {
            StUnit stUnit;
            Location location;
            Article article;
            StUnitContent stUnitContent;

            DBArticle dbArticle             = new DBArticle(_connection);
            DBLocation dbLocation           = new DBLocation(_connection);
            DBStUnit dbStUnit               = new DBStUnit(_connection);
            DBStUnitContent dbStUnitContent = new DBStUnitContent(_connection);

            // STUNIT 1 - BUFFER PHONES
            location = new Location() { Mha = "BUFFER", Rack = "1", HorCoor = "0", VerCoor = "0" };
            dbLocation.Get(ref location);

            stUnit = new StUnit() { StUnitType = "BUFFER_L", FK_LocationID = location.LocationID };
            dbStUnit.Create(ref stUnit);

            article = new Article() { Partno = "PHONE", Revision = "" };
            dbArticle.Get(ref article);

            stUnitContent = new StUnitContent() { FK_StUnitID = stUnit.StUnitID, FK_ArticleID = article.ArticleID, Quantity = 200 };
            dbStUnitContent.Create(ref stUnitContent);

            // STUNIT 2 - BUFFER LAPTOPS
            location = new Location() { Mha = "BUFFER", Rack = "1", HorCoor = "1", VerCoor = "0" };
            dbLocation.Get(ref location);

            stUnit = new StUnit() { StUnitType = "BUFFER_M", FK_LocationID = location.LocationID };
            dbStUnit.Create(ref stUnit);

            article = new Article() { Partno = "LAPTOP", Revision = "" };
            dbArticle.Get(ref article);

            stUnitContent = new StUnitContent() { FK_StUnitID = stUnit.StUnitID, FK_ArticleID = article.ArticleID, Quantity = 50 };
            dbStUnitContent.Create(ref stUnitContent);

            stUnit = new StUnit() { StUnitType = "BUFFER_M", FK_LocationID = location.LocationID };
            dbStUnit.Create(ref stUnit);

            article = new Article() { Partno = "LAPTOP", Revision = "REFURBISHED" };
            dbArticle.Get(ref article);

            stUnitContent = new StUnitContent() { FK_StUnitID = stUnit.StUnitID, FK_ArticleID = article.ArticleID, Quantity = 50 };
            dbStUnitContent.Create(ref stUnitContent);

            // STUNIT 3 - PICKING PHONES
            location = new Location() { Mha = "PICKING", Rack = "1", HorCoor = "0", VerCoor = "0" };
            dbLocation.Get(ref location);

            stUnit = new StUnit() { StUnitType = "PICKING_S", FK_LocationID = location.LocationID };
            dbStUnit.Create(ref stUnit);

            article = new Article() { Partno = "PHONE", Revision = "" };
            dbArticle.Get(ref article);

            stUnitContent = new StUnitContent() { FK_StUnitID = stUnit.StUnitID, FK_ArticleID = article.ArticleID, Quantity = 20 };
            dbStUnitContent.Create(ref stUnitContent);

            // STUNIT 4 - INBOUND FRIDGES
            location = new Location() { Mha = "INBOUND05", Rack = "", HorCoor = "", VerCoor = "" };
            dbLocation.Get(ref location);

            stUnit = new StUnit() { StUnitType = "PALLET", FK_LocationID = location.LocationID };
            dbStUnit.Create(ref stUnit);

            article = new Article() { Partno = "FRIDGE", Revision = "" };
            dbArticle.Get(ref article);

            stUnitContent = new StUnitContent() { FK_StUnitID = stUnit.StUnitID, FK_ArticleID = article.ArticleID, Quantity = 5 };
            dbStUnitContent.Create(ref stUnitContent);
        }
    }
}
