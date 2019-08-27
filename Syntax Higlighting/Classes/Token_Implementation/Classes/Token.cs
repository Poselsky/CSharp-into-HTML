using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Higlighting.Classes.Token_Implementation
{
    class Token
    {
        public string Text { get; private set; } // text tokenu
        public TokenType Type { get; set; } // typ tokenu
        public Tuple<int, int> Position { get; private set; } //nepovinné
        public Token PreviousToken { get; private set; }

        public Token(string text, TokenType type, Tuple<int, int> position, Token previousToken)
        {
            Text = text;
            Type = type;
            Position = position;
            PreviousToken = previousToken;
        }

        public override string ToString() // určeno pro ladící účely nikoliv fromátování
        {
            return string.Format("|{0}| ({1})  [ {2}, {3} ]", Text, Type, Position.Item1, Position.Item2);
        }

        public enum TokenType
        {
            //level 0 (jednoduché rozlišení mezi bílými znaky a slovy jazyka)
            SYMBOL,   // tento typ tokenu by se neměl ve výstupu skutečného tokenizátoru vyskytovat
            WHITE_SPACES,
            ESCAPE_CHAR,
            ESCAPE_CHAR_TAB,
            // level 1 (základní typy tokenů v C# a podobných jazycích)
            IDENTIFIER,  //libovolný identifikátor
            KEY_WORD,
            SEPARATOR,
            OPERATOR,
            LITERAL,
            INT_LITERAL,
            FLOAT_LITERAL,
            CHAR_LITERAL,
            STRING_LITERAL,
            COMMENT,
            // level 2 (detailnější typy identifikátorů, rozeznatelných jen podle okolních tokenů)
            IDENTIFIER_CRASH,
            IDENTIFIER_CLASS,  // jediný povinný z úrovně 2 (jméno třídy v definici)
            IDENTIFIER_NS, // jméno jmenného prostoru v konstrukcích using a namespace
            IDENTIFIER_METHOD, //jméno nově definované metody
            IDENTIFIER_PROPERTY // jméno nově definované vlastnosti
        }
    }


}
