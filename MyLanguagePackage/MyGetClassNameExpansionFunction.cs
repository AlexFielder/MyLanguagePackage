using Microsoft.VisualStudio.Package;

namespace MyLanguagePackage
{
    //==============================================================
    // The Expansion functions.

    //--------------------------------------------------------------
    // This expansion function returns the value of its argument.
    internal class MyGetClassNameExpansionFunction : ExpansionFunction
    {
        public MyGetClassNameExpansionFunction(ExpansionProvider provider)
            : base(provider)
        {
        }

        public override string GetCurrentValue()
        {
            string argValue = null;
            if (this.ExpansionProvider != null)
            {
                // The expansion session may not be initialized yet
                // so do not try to get any arguments if the session is
                // is null. In this case, the default value for the
                // associated field should be the same as the field
                // passed as the argument.
                if (this.ExpansionProvider.ExpansionSession != null)
                {
                    argValue = this.GetArgument(0);
                }
            }
            return argValue;
        }
    }


    //--------------------------------------------------------------
    // This expansion function returns a list of options.
    internal class MyEnumAccessTypeExpansionFunction : ExpansionFunction
    {
        string[] nameList = null;

        public MyEnumAccessTypeExpansionFunction(ExpansionProvider provider)
            : base(provider)
        {
        }

        public override string[] GetIntellisenseList()
        {
            if (nameList == null)
            {
                nameList = new string[] {
                    "public",
                    "protected",
                    "private",
                    "internal",
                };
            }
            return nameList;
        }

        public override string GetCurrentValue()
        {
            // Enumerations do not need to return a value.
            return null;
        }
    }


    ////==============================================================
    //// The language service, showing how the expansion functions are
    //// instantiated.

    ////--------------------------------------------------------------
    //public class MyLanguageService : LanguageService
    //{
    //    public override ExpansionFunction CreateExpansionFunction(ExpansionProvider provider,
    //                                                              string functionName)
    //    {
    //        ExpansionFunction function = null;
    //        if (String.Compare(functionName, "GetClassName", true) == 0)
    //        {
    //            function = new MyGetClassNameExpansionFunction(provider);
    //        }
    //        else if (String.Compare(functionName, "EnumAccessType", true) == 0)
    //        {
    //            function = new MyEnumAccessTypeExpansionFunction(provider);
    //        }
    //        return function;
    //    }
    //}
}