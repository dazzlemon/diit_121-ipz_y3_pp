using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Editor
{
    internal class WindowCommand : ICommand
    {

        public object sender { get; set; }
        public RoutedEventArgs e { get; set; }

        //Set this delegate when you initialize a new object. This is the method the command will execute. You can also change this delegate type if you need to.
        public Action<object, RoutedEventArgs> ExecuteDelegate { get; set; }

        //the important method that executes the actual command logic
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(sender, e);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        //always called before executing the command, mine just always returns true
        public bool CanExecute(object parameter)
        {
            return true; //mine always returns true, yours can use a new CanExecute delegate, or add custom logic to this method instead.
        }

        public event EventHandler CanExecuteChanged; //i'm not using this, but it's required by the interface
    }
}
