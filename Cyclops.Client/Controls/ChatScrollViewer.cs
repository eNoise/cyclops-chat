using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Cyclops.Client.ViewModel;

namespace Cyclops.Client.Controls
{
    public class ChatFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        /// <summary>
        /// Backing store for the <see cref="ScrollViewer"/> property.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        /// Gets the scroll viewer contained within the FlowDocumentScrollViewer control
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get
            {
                if (this.scrollViewer == null)
                {
                    DependencyObject obj = this;

                    do
                    {
                        if (VisualTreeHelper.GetChildrenCount(obj) > 0)
                            obj = VisualTreeHelper.GetChild(obj as Visual, 0);
                        else
                            return null;
                    }
                    while (!(obj is ScrollViewer));

                    this.scrollViewer = obj as ScrollViewer;
                }

                return this.scrollViewer;
            }
        }
    }

    public class ChatFlowDocument : FlowDocument
    {
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return (ObservableCollection<MessageViewModel>)GetValue(MessagesProperty); }
            set { SetValue(MessagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Messages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessagesProperty =
            DependencyProperty.Register("Messages", typeof(ObservableCollection<MessageViewModel>), typeof(ChatFlowDocument), new UIPropertyMetadata(OnInitializeMessagesStatic));

        private static void OnInitializeMessagesStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChatFlowDocument)
                ((ChatFlowDocument) d).OnInitializeMessages();
        }


        private void OnInitializeMessages()
        {
            Messages.ForEach(msg => Blocks.Add(msg.Paragraph));
            Messages.CollectionChanged += (s, e) => 
                                              {
                                                  if (e.Action == NotifyCollectionChangedAction.Add)
                                                      e.NewItems.OfType<MessageViewModel>().ForEach(
                                                          i => Blocks.Add(i.Paragraph));
                                                  ScrollToBottom();
                                              };
        }

        private void ScrollToBottom()
        {
            if (Parent is ChatFlowDocumentScrollViewer)
                ((ChatFlowDocumentScrollViewer) Parent).ScrollViewer.ScrollToBottom();
        }
    }
}
