using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cyclops.Client.Controls
{
    /// <summary>
    /// Text box with command on 'enter' button
    /// </summary>
    public class InputBox : TextBox
    {
        public ICommand SendCommand
        {
            get { return (ICommand)GetValue(SendCommandProperty); }
            set { SetValue(SendCommandProperty, value); }
        }

        public static readonly DependencyProperty SendCommandProperty =
            DependencyProperty.Register("SendCommand", typeof(ICommand), typeof(InputBox), new UIPropertyMetadata(null));

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (SendCommand != null && SendCommand.CanExecute(null) && e.Key == Key.Enter)
                SendCommand.Execute(null);
            base.OnKeyUp(e);
        }
    }
}
