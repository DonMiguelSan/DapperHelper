﻿using DapperHelper.Attributes;
using DapperHelper.Interfaces;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace DapperHelper.Tests
{
    internal class TestModel1 : IDbTableModel
    {
        [TableIdentity]
        public int Id { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        public bool TestOk { get; set; }

        [StoredProcedurePar]
        public DateTime CreatedOn { get; set; }

        [UpdatablePar]
        [StoredProcedurePar]
        public DateTime UpdatedOn { get; set; }

        public static string GetStringForQueryTesting(bool isValueString = false)
        {
            TestModel1 model = new();

            List<Type> attributte = new()
            {
                typeof(UpdatableParAttribute),
                typeof(StoredProcedureParAttribute)
            };

            return model.GetStringForQuery(attributte, isValueString);
        }

    }
}