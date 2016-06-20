namespace GuidGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft;

    public class SimpleCommand : CommandBase
    {
        private readonly Action command;

        public SimpleCommand(Action command)
        {
            Requires.NotNull(command, "command");
            this.command = command;
        }

        public override void Execute(object parameter)
        {
            this.command();
        }
    }
}
