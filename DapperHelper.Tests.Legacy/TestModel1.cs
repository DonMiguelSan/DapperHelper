using DapperHelper.Attributes;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace DapperHelper.Tests
{
    [TableName("IctTestResult")]
    internal class TestModel1
    {
        [KeyIndentity]
        public int Id { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        public int Result { get; set; }
        [StoredProcedurePar]
        public DateTime CreatedOn { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        [DateTimeFilter]
        public DateTime UpdatedOn { get; set; }

        public static string GetStringForQueryTesting(bool isValueString = false)
        {
            TestModel1 model = new TestModel1();

            List<Type> attributte = new List<Type>()
            {
                typeof(UpdatableParAttribute),
                typeof(StoredProcedureParAttribute)
            };

            return model.GetStringForQuery(attributte, isValueString);

        }

    }
}
