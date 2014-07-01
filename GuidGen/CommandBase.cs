namespace GuidGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Validation;

    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        protected virtual void OnCanExecuteChanged()
        {
            var changed = this.CanExecuteChanged;
            if (changed != null)
            {
                changed(this, EventArgs.Empty);
            }
        }
    }
}
