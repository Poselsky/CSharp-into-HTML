using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syntax_Higlighting.Classes.Token_Implementation;
using Microsoft.CSharp;
using System.Reflection;

namespace Syntax_Higlighting.Classes.Formater_Implementation.Classes
{
    class HTMLFormater : AbstractSHFormater
    {

        //When creating instance, this should not be a problem, with new instance comes new in-build properties
        private List<string> savedClassInstances { get; set; } = new List<string>();

        private StringBuilder formattedHTML = new StringBuilder();

        static bool Crashed = false;


        internal readonly Dictionary<Token.TokenType, Func<Token, string>> tokenTypeHtmlFormatter = new Dictionary<Token.TokenType, Func<Token, string>>()
        {
            {Token.TokenType.IDENTIFIER, (el) => "<span class=\"identifier\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">"+el.Text+"</span>"},
            {Token.TokenType.IDENTIFIER_CLASS, (el => "<span class=\"identifier_class\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">"+el.Text+"</span>")},
            {Token.TokenType.IDENTIFIER_NS, (el => "<span class=\"identifier_ns\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">"+el.Text+"</span>")},
            {Token.TokenType.KEY_WORD, (el) => "<span class=\"keyword\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">"+el.Text+"</span>"},
            {Token.TokenType.LITERAL, (el) => "<span class=\"literal\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">"+el.Text+"</span>" },
            {Token.TokenType.WHITE_SPACES, (el) => el.Text },
            {Token.TokenType.SYMBOL, (el) => "<span class=\"symbol\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">"+el.Text+"</span>" },
            {Token.TokenType.SEPARATOR, (el => "<span class=\"separator\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">" + el.Text + "</span>") },
            {Token.TokenType.OPERATOR, (el => "<span class=\"operator\" title=\"(" +el.Position.Item1+ ","+el.Position.Item2+ ")\">" + el.Text + "</span>")  },
            {Token.TokenType.IDENTIFIER_CRASH, (el => {
                    Crashed = true;
                    return "<span class =\"crash\"> There is error in line ( " + el.Position.Item1 + "," + el.Position.Item2+ " ) text: " + el.Text+ "</span>";
                })},
            {Token.TokenType.ESCAPE_CHAR, (el => el.Text )},
        };

        public override string Header()
        {
            return @"
                        <!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                          <meta charset = ""UTF-8"">
 
                           <title> Generated HTML </title>
                         </head>
                         <body>
                         <div class='wrapper'>
            ";
        }


        public override string Format(Token token)
        {
            /*
            return tokenTypeHtmlFormatter[token.Type](token.Text) + "<br>";
            return (""+token.Position.Item1+ " ," + token.Position.Item2+ "  |  "  + token.Text);
            */

            if (!Crashed)
            {
                return tokenTypeHtmlFormatter[token.Type](token);
            } else
            {
                return "";
            }

        }

        public override string Footer()
        {
            return
            "</div>"+
            "<link rel=\"stylesheet\" type=\"text/css\" href=\"./style.css\"" +
            "</body>" +
            "</html >";
        }

    }
}
