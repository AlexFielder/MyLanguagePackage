//------------------------------------------------------------------------------
// <copyright file="MyLanguagePackageEditorClassifierFormat.cs" company="Hewlett-Packard Company">
//     Copyright (c) Hewlett-Packard Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace MyLanguagePackage
{
    /// <summary>
    /// Defines an editor format for the MyLanguagePackageEditorClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "MyLanguagePackageEditorClassifier")]
    [Name("MyLanguagePackageEditorClassifier")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class MyLanguagePackageEditorClassifierFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyLanguagePackageEditorClassifierFormat"/> class.
        /// </summary>
        public MyLanguagePackageEditorClassifierFormat()
        {
            this.DisplayName = "MyLanguagePackageEditorClassifier"; // Human readable version of the name
            this.BackgroundColor = Colors.BlueViolet;
            this.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
}
