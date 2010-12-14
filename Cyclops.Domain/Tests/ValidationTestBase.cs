using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Cyclops.Core.Tests
{
    /// <summary>
    /// Base class for validation tests
    /// </summary>
    public abstract class ValidationTestBase<TData>
    {
        /// <summary>
        /// Creates a collection of test data
        /// </summary>
        protected void ValidateDataCollection<T>(Action<TData, T> propertySetter, T[] values, bool expectSuccess = true)
        {
            foreach (T value in values)
            {
                TData data = CreateValidData();
                propertySetter(data, value);

                if (expectSuccess)
                    Assert.IsTrue(Validate(data));
                else
                    Assert.IsFalse(Validate(data));
            }
        }

        /// <summary>
        /// Creates a simple valid data
        /// </summary>
        protected virtual TData CreateValidData()
        {
            return default(TData);
        }

        /// <summary>
        /// True, if config data is valid (all properties satisfied their validation rules)
        /// </summary>
        protected bool Validate(TData config)
        {
            var context = new ValidationContext(config, null, null);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(config, context, results);
        }
    }
}