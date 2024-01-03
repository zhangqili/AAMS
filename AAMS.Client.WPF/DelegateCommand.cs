using System.Windows.Input;

namespace AAMS.Client.WPF
{
    public class DelegateCommand : ICommand
    {
        public Action<object> ExecuteAction { get; set; }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public Func<object, bool> CanExecuteFunc { get; set; }
        public DelegateCommand()
        {

        }

        public DelegateCommand(Action<object> action)
        {
            ExecuteAction = action;
        }
    }
}
