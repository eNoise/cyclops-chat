using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Cyclops.Core;
using Cyclops.Core.Model;

namespace Cyclops.Client.MessageDecoration
{
    public class MessagePresenter
    {
        public static Paragraph Present(IConferenceMessage message)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SetResourceReference(Paragraph.StyleProperty, "parentRowStyle");

            // at first, textInlines will contains only one inline - raw text
            List<Inline> textInlines = new List<Inline>();

            // applying custom decorators
            DecoratorsRegistry.GetDecorators().ForEach(i => textInlines = i.Decorate(message, textInlines));

            paragraph.Inlines.AddRange(textInlines);

            return paragraph;
        }
    }
}
