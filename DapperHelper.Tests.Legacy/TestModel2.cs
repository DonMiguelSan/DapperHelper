using DapperHelper.Attributes;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace DapperHelper.Tests
{
    internal class TestModel2
    {
        [KeyIndentity]
        public int Id { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        public int TestingResult { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        public bool MyStatus { get; set; }

        [StoredProcedurePar]
        public DateTime CreatedOn { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        public DateTime UpdatedOn { get; set; }

        public static string GetStringForQueryTesting(bool isValueString = false)
        {
            TestModel2 model = new TestModel2();

            List<Type> attributte = new List<Type>()
            {
                typeof(UpdatableParAttribute),
                typeof(StoredProcedureParAttribute)
            };

            return model.GetStringForQuery(attributte, isValueString);
        }
    }
}
