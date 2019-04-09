﻿using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using ZuccBot.ZuccRPG;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using System.Collections;
using DSharpPlus.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

//**NOTE** .NET Core 2.0 is the recommended target framework. (I don't know how your opening this project)
//**NOTE** There may or may not be a 'README.txt', please check, if 'README.txt' isn't in the main folder (folder that contains ZuccBot.sln) then ignore this.

namespace ZuccBot
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;

        static void Main(string[] args)
        {
            Console.WriteLine("ZuccBot is awake.");
            Console.WriteLine(Directory.GetCurrentDirectory());
            //Rev this bad boy up (▀̿Ĺ̯▀̿ ̿)
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        //Setup the bot
        static async Task MainAsync(string[] args)
        {
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
        }


        //**NOTE** You can NOT serialize DiscordEmbedBuilders and cast them as DiscordEmbeds upon deserialization, you MUST serialize DiscordEmbeds as DiscordEmbeds, if you need to serialize a DiscordEmbed then construct it FIRST and then serialize, otherwise deserialization will NOT work.

        //This is some really janky code, you have been warned.
        //Most, if not, all of this code is related to paginated embeds as this is the event that's fired on every reaction
        //Ethan helped me with this part, the main problem I was having was that objects weren't serializing to the JSON properly and I was serializing DiscordEmbedBuilders and trying to cast them as a generic var and then construct a DiscordEmbed with that. (You normally create DiscordEmbeds this way, however this was the exception)
        private static async Task Discord_ReactionAdded(MessageReactionAddEventArgs e)
        {
            //This is gonna read the file thats being called (CharacterConfig.txt)
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\CharacterConfig.txt"))
            {
                //Row, row, row your boat, gently down the stream...

                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//We're gonna use this bad boy to read files from the current steam

                //List because theres multiple embeds and lists are easy to edit.
                List<DiscordEmbed> embed = (List<DiscordEmbed>)serializer.Deserialize(file, typeof(List<DiscordEmbed>));

                //**NOTE**There were problems with constants and switch statements and emojis, that's why this extremely convulated if statement exists
                //Check if the message embed is the same as the first listed embed in the txt file. (the embeds are listed on the txt file in the correct order they should be displayed, e.g. the first listed embed is the first embed that is called)
                if (e.Message.Embeds[0].Title == embed[0].Title)
                {
                    if (e.Message.Author.IsCurrent == e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot)
                    {
                        //Switch statements couldn't work due to Discord Emojis not being constant, among other things
                        if (e.Emoji == DiscordEmoji.FromName(discord, ":man:") || e.Emoji == DiscordEmoji.FromName(discord, ":leaves:") || e.Emoji == DiscordEmoji.FromName(discord, ":pick:"))
                        {
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
                            await e.Message.DeleteAllReactionsAsync();

                            //These emojis should correspond with the choices on the next page (page 2 / element 1)
                            await e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":crossed_swords:"));
                            await e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":bow_and_arrow:"));
                            await e.Message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":dagger:"));

                            //The task was completed successfully and needs to be closed
                            await Task.CompletedTask;
                        }
                        else
                        {
                            await Task.Delay(100);
                        }
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }
                else
                if (e.Message.Embeds[0].Title == embed[1].Title)
                {
                    if (e.Message.Author.IsCurrent == e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot)
                    {
                        if (e.Emoji == DiscordEmoji.FromName(discord, ":crossed_swords:") || e.Emoji == DiscordEmoji.FromName(discord, ":bow_and_arrow:") || e.Emoji == DiscordEmoji.FromName(discord, ":dagger:"))
                        {
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

                            //Were done with this message, delete it to clear clutter
                            await e.Message.DeleteAsync();

                            //The task was completed successfully and no longer needs to be active
                            await Task.CompletedTask;
                        }
                        else
                        {
                            await Task.Delay(100);
                        }
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
    }
}