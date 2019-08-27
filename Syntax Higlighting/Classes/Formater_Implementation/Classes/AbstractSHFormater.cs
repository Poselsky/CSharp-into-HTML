using Syntax_Higlighting.Classes.Token_Implementation;
using Syntax_Higlighting.Classes.Tokenizer_Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Higlighting.Classes.Formater_Implementation
{
    abstract class AbstractSHFormater : ISHFormater
    {
        TextWriter output;
        IEnumerable<Token> source;
        List<Token> preCompiledTokens = new List<Token>();

        public void setSource(IEnumerable<Token> source)
        {
            this.source = source;
        }

        public void setOutput(TextWriter output)
        {
            this.output = output;
        }

        public virtual void run()
        {
            StringBuilder outputedHTML = new StringBuilder();
            output.Write(Header()); //vypiš statickou hlavičku (např. neměnný počátek HTML)
            foreach (Token token in source)
            {
                //output.Write(Format(token)); // vypiš naformátovaný token
                preCompiledTokens.Add(token); // predkompilujeme tokeny
            }
            preCompiledTokens.ForEach(el=> outputedHTML.Append(Format(el)));
            output.Write(outputedHTML.ToString());
            output.Write(Footer()); //a fixní patičku např. </body></html>
        }

        // všechny metody volané z  `run` je nutno definovat v odvozených třídách
        public abstract string Header();
        public abstract string Format(Token tokens);
        public abstract string Footer();
    }
}
