using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyLanguagePackage
{
    public class TokenDefinition
    {
        private Regex _regex;
        private readonly MyTokenType _returnsToken;
        private readonly int _precedence;

        public TokenDefinition(MyTokenType returnsToken, string regexPattern, int precedence)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _returnsToken = returnsToken;
            _precedence = precedence;
        }

        public IEnumerable<TokenMatch> FindMatches(string inputString)
        {
            var matches = _regex.Matches(inputString);
            for (int i = 0; i < matches.Count; i++)
            {
                yield return new TokenMatch()
                {
                    StartIndex = matches[i].Index,
                    EndIndex = matches[i].Index + matches[i].Length,
                    TokenType = _returnsToken,
                    Value = matches[i].Value,
                    Precedence = _precedence
                };
            }
        }
    }
    //replaced AF 2017-02-13
    //public class TokenDefinition
    //{
    //    public TokenDefinition(
    //        string type,
    //        Regex regex)
    //        : this(type, regex, false)
    //    {
    //    }

    //    public TokenDefinition(
    //        string type,
    //        Regex regex,
    //        bool isIgnored)
    //    {
    //        Type = type;
    //        Regex = regex;
    //        IsIgnored = isIgnored;
    //    }

    //    public bool IsIgnored { get; private set; }
    //    public Regex Regex { get; private set; }
    //    public string Type { get; private set; }
    //}
}
