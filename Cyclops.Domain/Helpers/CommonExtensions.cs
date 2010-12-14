using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops
{
    public static class CommonExtensions
    {
        public static IEnumerable<string> SplitAndIncludeDelimiters(this string text, string[] delimiters)
        {
            if (delimiters.Length == 0)
            {
                yield return text;
                yield break;
            }

            var split = text.Split(delimiters, StringSplitOptions.None);

            foreach (string part in split)
            {
                if (!string.IsNullOrEmpty(part))
                    yield return part;
                text = text.Substring(part.Length);

                string delim = delimiters.FirstOrDefault(x => text.StartsWith(x));
                if (delim != null)
                {
                    if (!string.IsNullOrEmpty(delim))
                        yield return delim;
                    text = text.Substring(delim.Length);
                }
            }
        }
    }
}
