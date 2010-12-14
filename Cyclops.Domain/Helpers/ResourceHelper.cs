using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cyclops.Core.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Read a content from resource file in specific assembly 
        /// </summary>
        public static string ReadFromResource(string path, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(path);
            if (stream == null)
                throw new ArgumentException("Cannot find resource by path in " + assembly);
            using (StreamReader sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }
    }
}
