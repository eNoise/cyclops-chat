using System.Collections.Generic;

namespace Cyclops.Client.MessageDecoration
{
    public static class DecoratorsRegistry
    {
        public static IEnumerable<IMessageDecorator> GetDecorators()
        {
            return new IMessageDecorator[]
                       {
                           //Order is important!
                           new CommonMessageDecorator(),
                           new HyperlinkDecorator(),
                           new SmilesDecorator(),
                           new NickDecorator(),
                           new TimestampDecorator()
                       };
        }
    }
}