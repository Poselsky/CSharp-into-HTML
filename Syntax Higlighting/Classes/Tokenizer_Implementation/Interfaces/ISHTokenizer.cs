using Syntax_Higlighting.Classes.Token_Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Higlighting.Classes.Tokenizer_Implementation
{
    interface ISHTokenizer : IEnumerator<Token>, IEnumerable<Token>
    {
        void setInput(TextReader input);
    }
}
