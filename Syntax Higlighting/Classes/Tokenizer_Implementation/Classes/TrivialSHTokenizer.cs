using Syntax_Higlighting.Classes.Token_Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Higlighting.Classes.Tokenizer_Implementation.Classes
{
    class TrivialSHTokenizer : AbstractSHTokenizer
    {
        public override bool MoveNext()
        {
            int input = reader.Peek(); // vrátí znak z proudu, ale nepřečte ho tj. neodstraní z 
            if (input == -1) {
                return false; // vrátí příznak dosažení konce iterátoru
            } // pokud je dosažen konec proudu
            char c = Convert.ToChar(input); // nyní už je bezpečné převést data (v podobě číselné representace znaku) na znak
            string text;
            if (char.IsWhiteSpace(c))
            { // pokud je to bílý znak
                text = readWhileInClass(lc => char.IsWhiteSpace(lc)); // nači a vrať všechny následující bílé znaky
                Current = new Token(text, Token.TokenType.WHITE_SPACES, null,null); // a nastav příslušný token
            }
            else
            {
                text = readWhileInClass(lc => !char.IsWhiteSpace(lc)); // jinak čti nemezerové znaky
                Current = new Token(text, Token.TokenType.SYMBOL, null,null); // a nastav příslušný token
            }
            Console.WriteLine(text);
    
            return true; // signalizuje, že ještě nebyl dosažen konec (a token ve vlastnosti `Current` je platný
        }

        //čte dokud funkce `classifier` předaná jako parametr vrací true - classifier = predicate
        private string readWhileInClass(Func<char, bool> classifier)
        {
            StringBuilder s = new StringBuilder();
            int input;
            while ((input = reader.Peek()) != -1)
            {
                char c = Convert.ToChar(input);
                if (classifier(c))
                {
                    s.Append(c);
                    reader.Read();
                }
                else
                {
                    break;
                }
            }
            return s.ToString();
        }
    }
}
