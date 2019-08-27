using Syntax_Higlighting.Classes.Token_Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Higlighting.Classes.Formater_Implementation.Classes
{
    class DebugSHFormater : AbstractSHFormater
    {
        public override string Header()
        {
            return ""; // není potřeba žádné hlavičky
        }

        public override string Footer()
        {
            return ""; // ani patičky
        }

        public override string Format(Token token)
        {
            return token.ToString() + "\n"; // vypíše se informace o tokenu nálsedovaná 
        }
    }
}
