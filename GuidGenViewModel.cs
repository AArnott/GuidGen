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
    using Validation;

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
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    GetCodeSnippetFormatString(this.Format),
                    this.Value,
                    this.Value.ToString("B").ToUpperInvariant(),
                    this.Value.ToString("X").Replace("{", "").Replace("}", ""),
                    this.Value.ToString("D").ToUpperInvariant(),
                    this.value.ToString("X").Replace("{", "").Replace("}", "").Replace("0x", "&H"));
            }
        }

        public void NewGuid()
        {
            this.Value = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the formatting string that can be used to create the code snippet.
        /// </summary>
        /// <param name="format">The code snippet format desired.</param>
        /// <returns>A string ready to be formatted with arguments.</returns>
        /// <remarks>
        /// The returned formatting string assumes arguments will be supplied as follows:
        /// {0} Guid
        /// {1} "{XXXXXXXX-XXX-...}"
        /// {2} "0xabdcdfse, 0xdfsaefq, ..." with no curly braces
        /// {3} "XXXXXXX-XXX-XXXX-..."
        /// {4} &Habdcdfse, &Hdfsaefq, ..." with no curly braces
        /// </remarks>
        private static string GetCodeSnippetFormatString(CodeSnippetFormat format)
        {
            switch (format)
            {
                case CodeSnippetFormat.ImplementOleCreate:
                    return "// {1}\r\nIMPLEMENT_OLECREATE(<<class>>, <<external_name>>, {2});";
                case CodeSnippetFormat.DefineGuid:
                    return "// {1}\r\nDEFINE_GUID(<<name>>, {2});";
                case CodeSnippetFormat.StaticConstStructGuid:
                    return "// {1}\r\nstatic const GUID <<name>> = {0:X};";
                case CodeSnippetFormat.RegistryFormat:
                    return "{1}";
                case CodeSnippetFormat.GuidAttributeWithBrackets:
                    return "[Guid(\"{3}\")]";
                case CodeSnippetFormat.GuidAttributeWithAngleBrackets:
                    return "<Guid(\"{3}\")>";
                case CodeSnippetFormat.CSharpFieldDefinition:
                    return "// {1}\r\nstatic readonly Guid SomeGuid = new Guid({2});";
                case CodeSnippetFormat.VBFieldFieldDefinition:
                    return "' {1}\r\nShared ReadOnly SomeGuid As Guid = New Guid({4})";
                default:
                    throw new NotSupportedException();
            }
        }

        public enum CodeSnippetFormat
        {
            ImplementOleCreate,
            DefineGuid,
            StaticConstStructGuid,
            RegistryFormat,
            GuidAttributeWithBrackets,
            GuidAttributeWithAngleBrackets,
            CSharpFieldDefinition,
            VBFieldFieldDefinition,
        }
    }
}
