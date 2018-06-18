// Copyright (c) Microsoft. All rights reserved.

namespace GuidGen
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Generates code snippets based on GUIDs.
    /// </summary>
    public static class GuidCodeSnippetFormatter
    {
        /// <summary>
        /// Gets a formatted code snippet for the specified GUID.
        /// </summary>
        /// <param name="value">The GUID to format.</param>
        /// <param name="format">The desired format.</param>
        /// <returns>A string with the formatted GUID.</returns>
        public static string GetCodeSnippet(Guid value, CodeSnippetFormat format)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                GetCodeSnippetFormatString(format),
                value,
                value.ToString("B").ToUpperInvariant(),
                value.ToString("X").Replace("{", string.Empty).Replace("}", string.Empty),
                value.ToString("D").ToUpperInvariant(),
                value.ToString("X").Replace("{", string.Empty).Replace("}", string.Empty).Replace("0x", "&H"));
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
        /// {4} &amp;Habdcdfse, &amp;Hdfsaefq, ..." with no curly braces
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
    }
}
