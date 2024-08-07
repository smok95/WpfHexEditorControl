using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WpfHexEditor.Winform.Sample
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var parsedArgs = ParseArguments(args);

            string title = "";

            if(parsedArgs.ContainsKey("title"))
            {
                title = parsedArgs["title"];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(title));
        }

        static Dictionary<string, string> ParseArguments(string[] args)
        {
            var result = new Dictionary<string, string>();

            foreach(var arg in args)
            {
                if(arg.StartsWith("--"))
                {
                    var keyValue = arg.Substring(2).Split('=');
                    if(keyValue.Length == 2)
                    {
                        var key = keyValue[0];
                        var value = keyValue[1];
                        result[key] = value;
                    }
                }
            }

            return result;
        }
    }
}
