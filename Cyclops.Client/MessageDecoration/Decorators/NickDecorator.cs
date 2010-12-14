using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core.Model;

namespace Cyclops.Client.MessageDecoration
{
    public class NickDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            string style = "nickStyle";
            string nick = msg.Author;
            if (string.IsNullOrEmpty(msg.Author))
            {
                style = "systemNickStyle";
                nick = "System";
            }

            Run nickInline = new Run(string.Format("{0}: ", nick));
            nickInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

            inlines.Insert(0, nickInline);
            return inlines;
        }

        #endregion
    }
}
