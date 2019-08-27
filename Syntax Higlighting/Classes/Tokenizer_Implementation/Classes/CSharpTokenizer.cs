using Microsoft.CSharp;
using Syntax_Higlighting.Classes.Token_Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Syntax_Higlighting.Classes.Tokenizer_Implementation.Classes
{
    class CSharpTokenizer : AbstractSHTokenizer
    {
        public CSharpCodeProvider codeProvider = new CSharpCodeProvider();

        private int positionX = 0;
        private int positionY = 0;

        private List<string> classesSaver = new List<string>();
        internal Token lastToken;

        public CSharpTokenizer()
        {
            lastToken = null;

        }

        public override bool MoveNext()
        {
            int c = reader.Peek();

            if (c == -1)
            {
                return false; // vrátí příznak dosažení konce iterátoru
            } // pokud je dosažen konec proudu

            char randomChar = Convert.ToChar(c);

            string text;

            if (!char.IsLetterOrDigit(randomChar))
            {
                if (char.IsWhiteSpace(randomChar))
                {
                    //nacti vsechny mezery a operatory
                    text = readWhileInClass(lc => char.IsWhiteSpace(lc));
                }else
                {
                    //nacti po jednom znaku
                    text = randomChar.ToString();
                    reader.Read();
                }

            }
            else
            {
                text = readWhileInClass(lc => char.IsLetterOrDigit(lc)); // jinak čti nemezerové znaky

            }
            iteratePositionBasedOnSequence(text);
            Current = new Token(text, CS.DetermineTokenType(text, codeProvider, lastToken), new Tuple<int, int>(positionX, positionY), lastToken); // a nastav příslušný token
            lastToken = Current;

            return true; // signalizuje, že ještě nebyl dosažen konec (a token ve vlastnosti `Current` je platný
        }

        private void iteratePositionBasedOnSequence(string randomValue)
        {
            //checking escape strings - new line
            if(randomValue.Contains("\r") || randomValue.Contains("\n") || randomValue.Contains("\r\n"))
            {
                positionY++;
                positionX = 0;
            }
            //checking tab character
            else if(randomValue.Contains("\t"))
            {
                positionX += 4;
            }
            else
            {
                positionX++;
            }
        }

        //getting valid token type, from which we can determine another tokens => recursive function, 
        static internal Token GetNonWhiteSpaceToken(Token token)
        {
            if(token.PreviousToken != null)
            {
                if(token.PreviousToken.Type == Token.TokenType.WHITE_SPACES)
                {
                    return GetNonWhiteSpaceToken(token.PreviousToken);
                }else
                {
                    return token.PreviousToken;
                }
            } else
            {
                return null;
            }
        }

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

        internal class CS
        {
            public static List<string> validClasses = new List<string>();

            internal static readonly List<string> separatorList = new List<string>()
            {
                {"("},{")"},{"{"}, {"}"}, {"["}, {"]"}, {";"}, {","}, {"."}, {":"}
            };

            internal static readonly List<string> operatorList = new List<string>()
            {
                {"+"},{"-"},{"/"}, {"*"}, {"++"}, {"--"}, {"=="}, {"!="},{"*="}, {"+="}, {"-="}, {"/="},
                {"%="}, {">"}, {"<"}, {">="}, {"<="}, {"&&"}, {"||"}, {"!"}, {"&"}, {"is"}, {"as"}, {"sizeof"}, {"typeof"}, {"?:"}, {"="}
            };

            private static MethodInfo methIsKeyword;
            static CS()
            {
                using (CSharpCodeProvider cs = new CSharpCodeProvider())
                {
                    FieldInfo infoGenerator = cs.GetType().GetField("generator", BindingFlags.Instance | BindingFlags.NonPublic);

                    object gen = infoGenerator.GetValue(cs);
                    methIsKeyword = gen.GetType().GetMethod("IsKeyword", BindingFlags.Static | BindingFlags.NonPublic);
                }
            }
            public static bool IsKeyword(string input)
            {
                return Convert.ToBoolean(methIsKeyword.Invoke(null, new object[] { input.Trim() }));
            }


            public static Token.TokenType DetermineTokenType(string text, CSharpCodeProvider provider, Token previousToken)
            {

                if (IsKeyword(text) || text.ToLower() == "get" || text.ToLower() == "set")
                {
                    return Token.TokenType.KEY_WORD;
                }
                else if (provider.IsValidIdentifier(text))
                {
                    //determine token type based on previous token, if we have two identifiers in a row, then the first one must be Class
                    if(previousToken != null)
                    {
                        Token nonWhiteSpaceToken = GetNonWhiteSpaceToken(previousToken);

                        if(nonWhiteSpaceToken != null)
                        {
                            if (nonWhiteSpaceToken.Type == Token.TokenType.IDENTIFIER)
                            {
                                validClasses.Add(nonWhiteSpaceToken.Text);
                                nonWhiteSpaceToken.Type = Token.TokenType.IDENTIFIER_CLASS;
                            }
                            else if (nonWhiteSpaceToken.Text.ToLower() == "new" || nonWhiteSpaceToken.Text.ToLower() == "class" || validClasses.Exists(el => el == text))
                            {
                                validClasses.Add(text);
                                return Token.TokenType.IDENTIFIER_CLASS;
                            }
                            else if(nonWhiteSpaceToken.Text.ToLower() == "namespace")
                            {
                                return Token.TokenType.IDENTIFIER_NS;
                            }         
                        }
                    }
                    return Token.TokenType.IDENTIFIER;
                }
                else if (separatorList.Exists(el => el == text))
                {
                    return Token.TokenType.SEPARATOR;
                }
                else if (operatorList.Exists(el => el == text))
                {
                    return Token.TokenType.OPERATOR;
                }
                else if(Regex.IsMatch(text, Regex.Escape(@"\b\!#") + "\"" + Regex.Escape(@"\$%&'\(\)\*\+,-\./:;<=>\?@\[\\]\^_`\{\|}~\b")))
                {
                    return Token.TokenType.LITERAL;
                }
                else
                {
                    if(Regex.IsMatch(text, @"\b\[A-Za-z0-9]\b"))
                    {
                        return Token.TokenType.IDENTIFIER_CRASH;
                    }else if(text.Contains("\r") || text.Contains("\n"))
                    {
                        return Token.TokenType.ESCAPE_CHAR;
                    }

                    return Token.TokenType.WHITE_SPACES;
                }
            }
        }
    }
}
