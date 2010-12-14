using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.Helpers
{
    /// <summary>
    /// </summary>
    public static class ReflectionHelper
    {        
        /// <summary>
        /// Create a shallow copy of an obect 
        /// </summary>
        public static TNew ShallowCopyTo<TOld, TNew>(TOld old) where TNew : new()
        {
            TNew result = new TNew();
            Type oldType = typeof(TOld);
            Type newType = typeof(TNew);

            foreach (var property in old.GetType().GetProperties())
            {
                var value = oldType.GetProperty(property.Name).GetValue(old, null);

                var propertyOfNew = newType.GetProperty(property.Name);
                if (propertyOfNew != null)
                    propertyOfNew.SetValue(result, value, null);
            }
            return result;
        }
    }
}
