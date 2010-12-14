using System.Collections.Generic;
using System.Windows.Documents;
using Cyclops.Core.Model;

namespace Cyclops.Client.MessageDecoration
{
    public interface IMessageDecorator
    {
        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines);
    }
}