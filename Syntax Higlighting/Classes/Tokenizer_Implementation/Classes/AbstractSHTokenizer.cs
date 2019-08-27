using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syntax_Higlighting.Classes.Token_Implementation;

namespace Syntax_Higlighting.Classes.Tokenizer_Implementation
{
    abstract class AbstractSHTokenizer : ISHTokenizer
    {
        protected TextReader reader;
        public Token Current { get; protected set; }
        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        // jediná metoda kterou je nutno doimplementovat, přečte token z proudu a jeho representací 
        // (= instanci třídy vloží do vlastnosti Current)
        public abstract bool MoveNext();

        public void Dispose()
        {
            reader.Dispose();
        }

        public void Reset()
        {
            throw new NotImplementedException("Invalid method");
        }

        public void setInput(TextReader reader)
        {
            this.reader = reader;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public static List<Token> Compile(List<Token> preCompiledTokens)
        {
            throw new NotImplementedException();
        }
    }
}
