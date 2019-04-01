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
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

//**NOTE** There may or may not be a 'README.txt', please check, if 'README.txt' isn't in the main folder (folder that contains ZuccBot.sln) then ignore this.

namespace ZuccBot
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;

        static void Main(string[] args)
        {
            Console.WriteLine("Started Main(string[] args)...");
            //Rev this bad boy up (▀̿Ĺ̯▀̿ ̿)
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Ended Main(string[] args)");
        }

        //Setup the bot
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Started MainAsync(string[] args)...");

            //Create a new client instance (this is the bots 'id', I think this also helps seperate the bots client from any other local clients so that way it doesn't need to use another account as a proxy)
            discord = new DiscordClient(new DiscordConfiguration
            {
                //Don't look I'm exposed!
                Token = "NTUzNDYwMTEwMzI5MTE4NzIx.D2Rqeg.6ATVmDT5kF4XF_3YUTjMxZj8SDE",

                //You know the drill...
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                //What could this be UWU
                //I chose the period because it's easy to select on a phone and also one of the easiest buttons to reach from the bottom right side of the keyboard
                StringPrefix = "."//This is a period in case this is to small or looks weird on your machine
            });

            //This basically detects reactions, it's subscribing to a Task called 'Discord_ReactionAdded', it is listed below...
            discord.MessageReactionAdded += Discord_ReactionAdded;

            //Command subscriptions
            commands.RegisterCommands<Commands>();//General commands (Utility: banning, kicking, etc.)
            commands.RegisterCommands<GenericRPG>();//RPG commands (Create characters, attack entities, etc.)

            await discord.ConnectAsync();//Is anyone listening, am I all alone?
            await Task.Delay(-1);//Wait infinitely. Bot purgatory. >:)
            Console.WriteLine("Ended MainAsync(string[] args)");//It's all over!
        }


        //**NOTE** You can NOT serialize DiscordEmbedBuilders and cast them as DiscordEmbeds upon deserialization, you MUST serialize DiscordEmbeds as DiscordEmbeds, if you need to serialize a DiscordEmbed then construct it FIRST and then serialize, otherwise deserialization will NOT work.

        //This might change in the future.
        //Ethan helped me with this part, the main problem I was having was that the JsonSerializer wasn't created properly and I was serializing DiscordEmbedBuilders and trying to cast them as a generic var and then construct a DiscordEmbed with that. (You normally create DiscordEmbeds this way)
        private static Task Discord_ReactionAdded(MessageReactionAddEventArgs e)
        {
            //Big boi coming through...
            //This is gonna read the file thats being called (CharacterConfig.txt)
            using (StreamReader file = File.OpenText(@"C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.txt"))
            {
                //Hacking the Matrix... ̿̿ ̿̿ ̿̿ ̿'̿'\̵͇̿̿\з= ( ▀ ͜͞ʖ▀) =ε/̵͇̿̿/’̿’̿ ̿ ̿̿ ̿̿ ̿̿

                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//We're gonna use this bad boy to read files from the current filestream
                Console.WriteLine(tcr);
                
                //List because theres multiple embeds and lists are easy to edit.
                List<DiscordEmbed> embed = (List<DiscordEmbed>)serializer.Deserialize(file, typeof(List<DiscordEmbed>));

                //**NOTE** Paginated Embeds are currently done in a really janky way, at some point these will be redone.

                //There were problems with constants and switch statements and emojis, that's why this extremely convulated if statement exists
                //Check if the message embed is the same as the first listed embed in the txt file. (the embeds are listed on the txt file in the correct order they should be displayed, e.g. the first listed embed is the first embed that is called)
                //The basically makes sure that other actions don't conflict with every other embed, otherwise you may run into problems
                if (e.Message.Embeds[0].Title == embed[0].Title)
                {
                    if (e.Message.Author.IsCurrent == e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot)
                    {
                        /* The reason why theres an if statement to detect if any of the desired emojis have been selected 
                             * and then creates individual if statements depending on the emoji is because there were actions that applied to all of the conditions, yet
                             * some only applied to specific actions so I decided to just create a double if rather than create a serparate method for it.
                             */
                        if (e.Emoji == DiscordEmoji.FromName(discord, ":man:") || e.Emoji == DiscordEmoji.FromName(discord, ":leaves:") || e.Emoji == DiscordEmoji.FromName(discord, ":pick:"))
                        {
                            Console.WriteLine("last");
                            if (e.Emoji == DiscordEmoji.FromName(discord, ":man:"))
                            {

                            }
                            else
                            if (e.Emoji == DiscordEmoji.FromName(discord, ":leaves:"))
                            {

                            }
                            else
                            if (e.Emoji == DiscordEmoji.FromName(discord, ":pick:"))
                            {

                            }
                            //We're gonna flip the paginated embed to the next page
                            var msg = e.Message.ModifyAsync($"", embed[1]);

                            //Change the reactions to the appropiate ones for the next page
                            e.Message.DeleteAllReactionsAsync();
                            //These emojis should correspond with the choices on the next page (page 2 or element 1)
                            e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":crossed_swords:"));
                            e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":bow_and_arrow:"));
                            e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":dagger:"));

                            //The task was completed successfully and no longer needs to be active
                            return Task.CompletedTask;
                        }
                        else
                        {
                            return Task.Delay(100);
                        }
                    }
                    else
                    {
                        return Task.Delay(100);
                    }
                }
                else
                if (e.Message.Embeds[0].Title == embed[1].Title)
                {
                    if (e.Message.Author.IsCurrent == e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot)
                    {
                        if (e.Emoji == DiscordEmoji.FromName(discord, ":crossed_swords:") || e.Emoji == DiscordEmoji.FromName(discord, ":bow_and_arrow:") || e.Emoji == DiscordEmoji.FromName(discord, ":dagger:"))
                        {
                            Console.WriteLine("last");
                            if (e.Emoji == DiscordEmoji.FromName(discord, ":crossed_swords:"))
                            {

                            }
                            else
                            if (e.Emoji == DiscordEmoji.FromName(discord, ":bow_and_arrow:"))
                            {

                            }
                            else
                            if (e.Emoji == DiscordEmoji.FromName(discord, ":dagger:"))
                            {

                            }
                            //We're gonna flip the paginated embed to the next page
                            var msg = e.Message.ModifyAsync($"", embed[2]);

                            //Change the reactions to the appropiate ones for the next page
                            e.Message.DeleteAllReactionsAsync();
                            //These emojis should correspond with the choices on the next page (page 2 or element 1)
                            /*e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":crossed_swords:"));
                            e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":bow_and_arrow:"));
                            e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":dagger:"));*/

                            //The task was completed successfully and no longer needs to be active
                            return Task.CompletedTask;
                        }
                        else
                        {
                            return Task.Delay(100);
                        }
                    }
                    else
                    {
                        return Task.Delay(100);
                    }
                }
                else
                {
                    return Task.Delay(100);
                }
            }
        }
    }
}
