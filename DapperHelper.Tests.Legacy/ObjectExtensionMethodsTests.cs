using DapperHelper.Attributes;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace DapperHelper.Tests
{
    public class ObjectExtensionMethodsTests
    {
        [Theory]
        [InlineData("icttestresult(Result, UpdatedOn, CreatedOn)", "TestModel2(TestingResult, MyStatus, UpdatedOn, CreatedOn)", false)]
        [InlineData("Values('0', '0001-01-01t00:00:00', '0001-01-01t00:00:00')", "Values('0', 'false', '0001-01-01t00:00:00', '0001-01-01t00:00:00')", true)]
        internal void TestGetStringForQueryForNotValues(string expectedModel1, string expectedModel2, bool isValueString)
        {

            string actualModel1 = TestModel1.GetStringForQueryTesting(isValueString);
            string actualModel2 = TestModel2.GetStringForQueryTesting(isValueString);

            Assert.Equal(expectedModel1.ToLower(), actualModel1.ToLower());
            Assert.Equal(expectedModel2.ToLower(), actualModel2.ToLower());
        }

        [Theory]
        [InlineData("Insert into dbo.IctTestResult(Result, UpdatedOn) Values('0', '0001-01-01t00:00:00')", "Insert into dbo.TestModel2(TestingResult, MyStatus, UpdatedOn) Values('0', 'false', '0001-01-01t00:00:00')")]
        internal void TestGetQueryStringForSqlTrans(string expectedModel1, string expectedModel2)
        {
            string actualModel1 = (new TestModel1()).GetQueryStringForSqlTrans();
            string actualModel2 = (new TestModel2()).GetQueryStringForSqlTrans();

            Assert.Equal(expectedModel1.ToLower(), actualModel1.ToLower());
            Assert.Equal(expectedModel2.ToLower(), actualModel2.ToLower());
        }

        [Fact]
        internal void TestGetTable()
        {
            List<TestModel1> data = new List<TestModel1>();
            TestModel1 testModel1 = new TestModel1();
            data.Add(new TestModel1() { CreatedOn = DateTime.Now });
            DataTable dataTable = data.GetDataTable();
            DataTable dataTableForTestModel1 = testModel1.GetDataTable();
            Assert.NotNull(dataTable);
            Assert.Equal(4, dataTable.Columns.Count);
            Assert.Equal(1, dataTable.Rows.Count);
            Assert.Equal(4, dataTableForTestModel1.Columns.Count);
            Assert.Equal(1, dataTableForTestModel1.Rows.Count);
        }

        [Fact]
        internal void TestGetColumnName()
        {
            TestModel1 testModel1 = new TestModel1();
            TestModel2 testModel2 = new TestModel2();
            Assert.Equal("UpdatedOn", testModel1.GetColumnName<DateTimeFilterAttribute>());
            Assert.Equal("", testModel2.GetColumnName<DateTimeFilterAttribute>());
        }
       

    }
}
