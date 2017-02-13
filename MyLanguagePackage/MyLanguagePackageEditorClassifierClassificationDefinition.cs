//------------------------------------------------------------------------------
// <copyright file="MyLanguagePackageEditorClassifierClassificationDefinition.cs" company="Hewlett-Packard Company">
//     Copyright (c) Hewlett-Packard Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace MyLanguagePackage
{
    /// <summary>
    /// Classification type definition export for MyLanguagePackageEditorClassifier
    /// </summary>
    internal static class MyLanguagePackageEditorClassifierClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

        /// <summary>
        /// Defines the "MyLanguagePackageEditorClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("MyLanguagePackageEditorClassifier")]
        private static ClassificationTypeDefinition typeDefinition;

#pragma warning restore 169
    }
}
