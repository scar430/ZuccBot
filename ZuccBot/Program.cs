//Seth Banker
//ZuccBot 0.0.1

/* If your wondering why I didn't use the Linq library for dealing with array elements it's because there's no point in importing a whole other library and using it's methods to do the equivalent of just checking an int, also it allows more "copy pasta" flexibility. (I'm informing you of this assuming your bots are written in JavaScript) 
 * TL;DR : No need to over complicate things plus it's easier to translate to other languages.
 */

using System;

namespace DiscordBot.Core
{
    internal class Program
    {
        private DiscordSocketClient _ client;

        private static void Main(string[] args)
        {
            Console.WriteLine("DEBUG : Starting Program.Main(string[] args) ...");

            //Check if args was populated...
            if(args.Length > 0)
            {
                //Check that the first element in args was the version number, if not, something is wrong.
                if (args[0] == "--version")
                {
                    Console.WriteLine("Information : Version Number : 0.0.1");
                }
                else
                {
                    Console.WriteLine("ERROR : args[0] not found in Program.Main(string[] args)");
                }
            }
            Console.WriteLine("DEBUG : Ending Program.Main(string[] args)");
        }
    }
}
