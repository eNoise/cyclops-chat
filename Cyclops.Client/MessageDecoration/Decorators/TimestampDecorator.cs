using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core.Model;

namespace Cyclops.Client.MessageDecoration
{
    public class TimestampDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            string style = "timestampStyle";
            var timestampInline = new Run(msg.Timestamp.ToString("[hh:mm:ss] "));
            timestampInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

            inlines.Insert(0, timestampInline);
            return inlines;
        }

        #endregion
    }
}