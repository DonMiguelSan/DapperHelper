using DapperHelper.Tools;
using System;
using Xunit;

namespace DapperHelper.Tests
{
    public class EnumsTests
    {
        [Theory]
        [InlineData("Create")]
        [InlineData("Insert")]
        [InlineData("Update")]
        [InlineData("Delete")]
        internal void GetOperationStringTest(string expected)
        {
            SqlActions operation = (SqlActions)Enum.Parse(typeof(SqlActions), expected);

            Assert.True(operation.ToString().Equals(expected));
        }
    }
}
