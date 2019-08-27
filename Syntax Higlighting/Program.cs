using Syntax_Higlighting.Classes.Formater_Implementation;
using Syntax_Higlighting.Classes.Formater_Implementation.Classes;
using Syntax_Higlighting.Classes.Tokenizer_Implementation;
using Syntax_Higlighting.Classes.Tokenizer_Implementation.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TidyManaged;


namespace Syntax_Higlighting
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // základní mechanismus použití (v minimální implementaci se budou lišit jen třídy tokenizeru a formátoru)
            ISHTokenizer t = new CSharpTokenizer();  // vytvoříme tokenizer            

            StreamReader reader;
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = "cs";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                reader = new StreamReader(fd.OpenFile());
            } else
            {
                throw new Exception("File not selected");
            }

            t.setInput(reader); // nastavíme mu vstupní proud
            ISHFormater f = new HTMLFormater(); // vytvoříme formátor
            f.setSource(t); // nastavíme mu tokenizer jako vstupní zdroj tokenů

            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            f.setOutput(writer); // nastavíme mu výstupní proud
            f.run(); // a spustíme proces zvýrazňování syntaxe

            StreamWriter file = new StreamWriter(@"syntax/syntax.html");
            file.Write(writer);

            writer.Close();
            file.Close();

        }
    }

    
}
