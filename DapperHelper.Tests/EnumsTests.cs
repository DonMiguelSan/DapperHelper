using DapperHelper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            SqlCommands operation = (SqlCommands)Enum.Parse(typeof(SqlCommands), expected);

            Assert.True(operation.ToString().Equals(expected));
        }
    }
}
