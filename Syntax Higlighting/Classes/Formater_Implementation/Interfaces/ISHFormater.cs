using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syntax_Higlighting.Classes.Token_Implementation;

namespace Syntax_Higlighting.Classes.Formater_Implementation
{
    interface ISHFormater
    {
        void setSource(IEnumerable<Token> source);
        void setOutput(TextWriter output);
        void run(); // provede naformátování všech tokenů
    }
}
