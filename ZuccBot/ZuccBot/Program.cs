using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using ZuccBot.ZuccRPG;

namespace ZuccBot
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;

        static void Main(string[] args)
        {
            Console.WriteLine("Started Main(string[] args)...");
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Ended Main(string[] args)");
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Started MainAsync(string[] args)...");
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NTUzNDYwMTEwMzI5MTE4NzIx.D2Rqeg.6ATVmDT5kF4XF_3YUTjMxZj8SDE",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = ">"
            });

            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<GenericRPG>();

            await discord.ConnectAsync();//Is anyone listening, am I all alone?
            await Task.Delay(-1);//Wait infinitely. Bot purgatory.
            Console.WriteLine("Ended MainAsync(string[] args)");
        }
    }
}
