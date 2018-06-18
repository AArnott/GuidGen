// Copyright (c) Microsoft. All rights reserved.

namespace GuidGen
{
    /// <summary>
    /// The various formats that the generated GUID can take.
    /// </summary>
    public enum CodeSnippetFormat
    {
        /// <summary>
        /// IMPLEMENT_OLECREATE(CLASS, EXTERNAL_NAME, 0x345c8354,0xdd80,0x4143,0xac,0xad,0x23,0x00,0x9c,0x09,0x29,0xa8);
        /// </summary>
        ImplementOleCreate,

        /// <summary>
        /// DEFINE_GUID(NAME_HERE, 0x345c8354,0xdd80,0x4143,0xac,0xad,0x23,0x00,0x9c,0x09,0x29,0xa8);
        /// </summary>
        DefineGuid,

        /// <summary>
        /// static const GUID NAME_HERE = {0x345c8354,0xdd80,0x4143,{0xac,0xad,0x23,0x00,0x9c,0x09,0x29,0xa8}};
        /// </summary>
        StaticConstStructGuid,

        /// <summary>
        /// {345C8354-DD80-4143-ACAD-23009C0929A8}
        /// </summary>
        RegistryFormat,

        /// <summary>
        /// [Guid("345C8354-DD80-4143-ACAD-23009C0929A8")]
        /// </summary>
        GuidAttributeWithBrackets,

        /// <summary>
        /// &lt;Guid("345C8354-DD80-4143-ACAD-23009C0929A8")&gt;
        /// </summary>
        GuidAttributeWithAngleBrackets,

        /// <summary>
        /// static readonly Guid SomeGuid = new Guid(0x345c8354, 0xdd80, 0x4143, 0xac, 0xad, 0x23, 0x00, 0x9c, 0x09, 0x29, 0xa8);
        /// </summary>
        CSharpFieldDefinition,

        /// <summary>
        /// Shared ReadOnly SomeGuid As Guid = New Guid(&amp;H345c8354,&amp;Hdd80,&amp;H4143,&amp;Hac,&amp;Had,&amp;H23,&amp;H00,&amp;H9c,&amp;H09,&amp;H29,&amp;Ha8)
        /// </summary>
        VBFieldFieldDefinition,
    }
}
