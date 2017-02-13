using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyLanguagePackage
{
    public class Lexer : ILexer
    {
        Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
        IList<TokenDefinition> tokenDefinitions = new List<TokenDefinition>();

        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            tokenDefinitions.Add(tokenDefinition);
        }

        public IEnumerable<Token> Tokenize(string errorMessage)
        {
            var tokenMatches = FindTokenMatches(errorMessage);

            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                .OrderBy(x => x.Key)
                .ToList();

            TokenMatch lastMatch = null;
            for (int i = 0; i < groupedByIndex.Count; i++)
            {
                var bestMatch = groupedByIndex[i].OrderBy(x => x.Precedence).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                    continue;

                yield return new Token(bestMatch.TokenType, bestMatch.Value);

                lastMatch = bestMatch;
            }

            yield return new Token(MyTokenType.SequenceTerminator);
        }

        private List<TokenMatch> FindTokenMatches(string errorMessage)
        {
            var tokenMatches = new List<TokenMatch>();

            foreach (var tokenDefinition in tokenDefinitions)
                tokenMatches.AddRange(tokenDefinition.FindMatches(errorMessage).ToList());

            return tokenMatches;
        }

        //replaced AF 2017-02-13
        //public IEnumerable<Token> Tokenize(string source)
        //{
        //    int currentIndex = 0;
        //    int currentLine = 1;
        //    int currentColumn = 0;

        //    while (currentIndex < source.Length)
        //    {
        //        TokenDefinition matchedDefinition = null;
        //        int matchLength = 0;

        //        foreach (var rule in tokenDefinitions)
        //        {
        //            var match = rule.Regex.Match(source, currentIndex);

        //            if (match.Success && (match.Index - currentIndex) == 0)
        //            {
        //                matchedDefinition = rule;
        //                matchLength = match.Length;
        //                break;
        //            }
        //        }

        //        if (matchedDefinition == null)
        //        {
        //            throw new Exception(string.Format("Unrecognized symbol '{0}' at index {1} (line {2}, column {3}).", source[currentIndex], currentIndex, currentLine, currentColumn));
        //        }
        //        else
        //        {
        //            var value = source.Substring(currentIndex, matchLength);

        //            if (!matchedDefinition.IsIgnored)
        //                yield return new Token(matchedDefinition.Type, value, new TokenPosition(currentIndex, currentLine, currentColumn));

        //            var endOfLineMatch = endOfLineRegex.Match(value);
        //            if (endOfLineMatch.Success)
        //            {
        //                currentLine += 1;
        //                currentColumn = value.Length - (endOfLineMatch.Index + endOfLineMatch.Length);
        //            }
        //            else
        //            {
        //                currentColumn += matchLength;
        //            }

        //            currentIndex += matchLength;
        //        }
        //    }

        //    yield return new Token("(end)", null, new TokenPosition(currentIndex, currentLine, currentColumn));
        //}

        internal string GetNextToken()
        {
            throw new NotImplementedException();
        }
    }
}
