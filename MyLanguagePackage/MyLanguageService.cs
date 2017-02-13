using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using System.Collections;
using Microsoft.VisualStudio.Shell;

namespace MyLanguagePackage
{
    class MyLanguageService : LanguageService
    {
        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string GetFormatFilterList()
        {
            throw new NotImplementedException();
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            throw new NotImplementedException();
        }

        public override IScanner GetScanner(IVsTextLines buffer)
        {
            throw new NotImplementedException();
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            return new TestAuthoringScope();
        }
        //added by AF 2017-02-13 - doesn't work!
        //private ColorableItem[] m_colorableItems;


        //TestLanguageService() : base()  
        //{
        //    m_colorableItems = new ColorableItem[] {
        //        new ColorableItem("TestLanguage – Text",
        //                          "Text",
        //                          COLORINDEX.CI_SYSPLAINTEXT_FG,
        //                          COLORINDEX.CI_SYSPLAINTEXT_BK,
        //                          System.Drawing.Color.Empty,
        //                          System.Drawing.Color.Empty,
        //                          FONTFLAGS.FF_DEFAULT),
        //        new ColorableItem("TestLanguage – Keyword",
        //                          "Keyword",
        //                          COLORINDEX.CI_MAROON,
        //                          COLORINDEX.CI_SYSPLAINTEXT_BK,
        //                          System.Drawing.Color.FromArgb(192,32,32),
        //                          System.Drawing.Color.Empty,
        //                          FONTFLAGS.FF_BOLD),
        //        new ColorableItem("TestLanguage – Comment",
        //                          "Comment",
        //                          COLORINDEX.CI_DARKGREEN,
        //                          COLORINDEX.CI_LIGHTGRAY,
        //                          System.Drawing.Color.FromArgb(32,128,32),
        //                          System.Drawing.Color.Empty,
        //                          FONTFLAGS.FF_DEFAULT)  
        //        // ...  
        //        // Add as many colorable items as you want to support.  
        //    };
        //}

        #region Snippets

        private ArrayList expansionsList;

        public override void OnParseComplete(ParseRequest req)
        {
            if (this.expansionsList == null)
            {
                this.expansionsList = new ArrayList();
                GetSnippets(this.GetLanguageServiceGuid(),
                    ref this.expansionsList);
            }
        }

        private void GetSnippets(Guid languageGuid,
                                 ref ArrayList expansionsList)
        {
            IVsTextManager textManager = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
            if (textManager != null)
            {
                IVsTextManager2 textManager2 = (IVsTextManager2)textManager;
                if (textManager2 != null)
                {
                    IVsExpansionManager expansionManager = null;
                    textManager2.GetExpansionManager(out expansionManager);
                    if (expansionManager != null)
                    {
                        // Tell the environment to fetch all of our snippets.  
                        IVsExpansionEnumeration expansionEnumerator = null;
                        expansionManager.EnumerateExpansions(languageGuid,
                        0,     // return all info  
                        null,    // return all types  
                        0,     // return all types  
                        1,     // include snippets without types  
                        0,     // do not include duplicates  
                        out expansionEnumerator);
                        if (expansionEnumerator != null)
                        {
                            // Cache our expansions in a VsExpansion array   
                            uint count = 0;
                            uint fetched = 0;
                            VsExpansion expansionInfo = new VsExpansion();
                            IntPtr[] pExpansionInfo = new IntPtr[1];

                            // Allocate enough memory for one VSExpansion structure. This memory is filled in by the Next method.  
                            pExpansionInfo[0] = Marshal.AllocCoTaskMem(Marshal.SizeOf(expansionInfo));

                            expansionEnumerator.GetCount(out count);
                            for (uint i = 0; i < count; i++)
                            {
                                expansionEnumerator.Next(1, pExpansionInfo, out fetched);
                                if (fetched > 0)
                                {
                                    // Convert the returned blob of data into a structure that can be read in managed code.  
                                    expansionInfo = (VsExpansion)Marshal.PtrToStructure(pExpansionInfo[0], typeof(VsExpansion));

                                    if (!String.IsNullOrEmpty(expansionInfo.shortcut))
                                    {
                                        expansionsList.Add(expansionInfo);
                                    }
                                }
                            }
                            Marshal.FreeCoTaskMem(pExpansionInfo[0]);
                        }
                    }
                }
            }
        }

        internal void AddSnippets(ref TestAuthoringScope scope)
        {
            if (this.expansionsList != null && this.expansionsList.Count > 0)
            {
                int count = this.expansionsList.Count;
                for (int i = 0; i < count; i++)
                {
                    VsExpansion expansionInfo = (VsExpansion)this.expansionsList[i];
                    scope.AddDeclarations(new TestDeclaration(expansionInfo.title,
                         expansionInfo.description,
                         "snippet"));
                }
            }
        }

        #endregion
    }

    internal class TestAuthoringScope : AuthoringScope
    {
        private List<TestDeclaration> m_declarations;

        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            span = new TextSpan();
            return null;
        }

        public override Declarations GetDeclarations(IVsTextView view,
                                                     int line,
                                                     int col,
                                                     TokenInfo info,
                                                     ParseReason reason)
        {
            return null;
        }

        public override string Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span)
        {
            span = new TextSpan();
            return null;
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            return null;
        }

        public void AddDeclarations(TestDeclaration declaration)
        {
            if (m_declarations == null)
                m_declarations = new List<TestDeclaration>();
            m_declarations.Add(declaration);
        }
    }

    class TestDeclaration
    {
        private string m_name;
        private string m_description;
        private string m_type;

        public TestDeclaration(string name, string desc, string type)
        {
            m_name = name;
            m_description = desc;
            m_type = type;
        }
    }

    public class Parser
    {
        private IList<TextSpan[]> m_braces;
        public IList<TextSpan[]> Braces
        {
            get { return m_braces; }
        }
        private void AddMatchingBraces(TextSpan braceSpan1, TextSpan braceSpan2)
        {
            if (IsMatch(braceSpan1, braceSpan2))
                 m_braces.Add(new TextSpan[] { braceSpan1, braceSpan2 });
        }

        private bool IsMatch(TextSpan braceSpan1, TextSpan braceSpan2)
        {
            return false;
            //definition for matching here  
        }
    }
}