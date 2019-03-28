using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using ZuccBot.ZuccRPG;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using ZuccBot.ZuccRPG.Generic;
using System.Collections;
using DSharpPlus.Net;
using ZuccBot.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

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
                StringPrefix = "."
            });

            discord.MessageReactionAdded += Discord_ReactionAdded;
            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<GenericRPG>();

            await discord.ConnectAsync();//Is anyone listening, am I all alone?
            await Task.Delay(-1);//Wait infinitely. Bot purgatory. >:)
            Console.WriteLine("Ended MainAsync(string[] args)");
        }

        private static Task Discord_ReactionAdded(MessageReactionAddEventArgs e)
        {
            if(e.Message.Author.IsCurrent == e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot && e.Emoji == DiscordEmoji.FromName((DiscordClient)e.Client, ":man:"))
            {
                Console.WriteLine("Message Recieved");
                e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":man:"));
                e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":leaves:"));
                e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":pick:"));
                return Task.CompletedTask;
            }
            else
            {
                Console.WriteLine("Message Delayed");
                return Task.Delay(25);
            }
        }
    }
}
