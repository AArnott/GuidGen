namespace GuidGen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft;

    public class GuidGenViewModel : BindableBase
    {
        public GuidGenViewModel()
        {
            this.NewGuidCommand = new SimpleCommand(() => this.NewGuid());
            this.CopyCommand = new SimpleCommand(() => Clipboard.SetText(this.CodeSnippet));

            this.Value = Guid.NewGuid();
            this.Format = CodeSnippetFormat.RegistryFormat;
            this.RegisterDependentProperty(() => Value, () => CodeSnippet);
            this.RegisterDependentProperty(() => Format, () => CodeSnippet);
        }

        public ICommand NewGuidCommand { get; private set; }

        public ICommand CopyCommand { get; private set; }

        private Guid value;
        public Guid Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value); }
        }

        private CodeSnippetFormat format;
        public CodeSnippetFormat Format
        {
            get { return this.format; }
            set { this.SetProperty(ref this.format, value); }
        }

        public string CodeSnippet
        {
            get { return GuidCodeSnippetFormatter.GetCodeSnippet(this.Value, this.Format); }
        }

        public void NewGuid()
        {
            this.Value = Guid.NewGuid();
        }
    }
}
