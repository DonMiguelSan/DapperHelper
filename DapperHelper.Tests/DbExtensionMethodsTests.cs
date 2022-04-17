using DapperHelper.Attributes;
using DapperHelper.Interfaces;
using DapperHelper.Tools;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DapperHelper.Tests
{
    public class DbExtensionMethodsTests
    {
        [Theory]
        [InlineData("TestModel1(TestOk, UpdatedOn, CreatedOn)", "TestModel2(TestingResult, MyStatus, UpdatedOn, CreatedOn)", false)]
        [InlineData("Values(@TestOk, @UpdatedOn, @CreatedOn)", "Values(@TestingResult, @MyStatus, @UpdatedOn, @CreatedOn)", true)]
        internal void TestGetStringForQueryForNotValues(string expectedModel1, string expectedModel2, bool isValueString)
        {

            string actualModel1 = TestModel1.GetStringForQueryTesting(isValueString);
            string actualModel2 = TestModel2.GetStringForQueryTesting(isValueString); 

            Assert.Equal(expectedModel1, actualModel1);
            Assert.Equal(expectedModel2, actualModel2);
        }

        [Theory]
        [InlineData("Insert TestModel1(TestOk, UpdatedOn) Values(@TestOk, @UpdatedOn)", "Insert TestModel2(TestingResult, MyStatus, UpdatedOn) Values(@TestingResult, @MyStatus, @UpdatedOn)", SqlCommands.Insert )]
        internal void TestGetQueryStringForSqlTrans(string expectedModel1, string expectedModel2, SqlCommands command)
        {
            string actualModel1 = (new TestModel1()).GetQueryStringForSqlTrans(command);
            string actualModel2 = (new TestModel2()).GetQueryStringForSqlTrans(command);    

            Assert.Equal(expectedModel1, actualModel1); 
            Assert.Equal (expectedModel2, actualModel2);    
        }
    }
}
