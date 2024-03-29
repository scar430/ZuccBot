﻿using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using ZuccBot.ZuccRPG;
using ZuccBot.ZuccRPG.RPGassets;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using DSharpPlus.CommandsNext.Converters;

//**NOTE** .NET Core 2.0 is the recommended target framework. (I don't know how your opening this project)
//**NOTE** As a general rule of thumb, if your wondering why something was done a certain way or seems inefficient (e.g. Everything and anything that deals with emojis.), it's usually due to some problem working with the DSharpPlus API.

namespace ZuccBot
{
    public class Program
    {
        //Discord Client for Mr. Zucc
        public static DiscordClient discord;
        //All registered commands
        static CommandsNextModule commands;

        //This is read from physical memory on startup and written into physical memory on shutdown
        public static Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>> players = new Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>>();
        public static Dictionary<ulong, List<Location>> locations = new Dictionary<ulong, List<Location>>();

        //Log on/off state (off by default)
        public static bool logEnabled = false;

        //Ringleader
        static void Main(string[] args)
        {
            Console.WriteLine("ZuccBot is awake.");
            Console.WriteLine(Directory.GetCurrentDirectory());//Announce where ZuccBot can be found. Please visit him he's lonely :(

            //Rev this bad boy up (▀̿Ĺ̯▀̿ ̿)
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        //Setup the bot
        static async Task MainAsync(string[] args)
        {
            //Create the bots client
            discord = new DiscordClient(new DiscordConfiguration
            {
                //As of 2/6/2023 I removed the token from this project since it is publicly displayed, in a functioning version of this you will need the token.
                //Don't look I'm exposed!
                //Token = [DELETED]",

                //You know the drill...
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                //Ask me anything
                StringPrefix = ">",
                EnableDefaultHelp = false
            });

            //A list of events the client is subscribed to.
            discord.MessageReactionAdded += Discord_ReactionAdded;
            discord.MessageCreated += Discord_MessageCreated;
            discord.MessageDeleted += Discord_MessageDeleted;
            discord.GuildMemberAdded += Discord_MemberAdded;
            discord.GuildMemberRemoved += Discord_MemberRemoved;
            discord.GuildCreated += Discord_GuildCreated;
            discord.GuildDeleted += Discord_GuildDeleted;
            discord.GuildUpdated += Discord_GuildUpdated;

            //Valid Commands.
            commands.RegisterCommands<Commands>();//General commands (banning, kicking, etc.)
            commands.RegisterCommands<GenericRPG>();//RPG commands (Create characters, attack entities, etc.)

            await discord.ConnectAsync();//Is anyone listening, am I all alone?
            await rpgInit(true);
            await Task.Delay(-1);//Wait infinitely. Bot purgatory.
        }

        private static async Task Discord_GuildUpdated(GuildUpdateEventArgs e)
        {
            await Task.CompletedTask;
        }

        private static async Task Discord_GuildDeleted(GuildDeleteEventArgs e)
        {
            await Task.CompletedTask;
        }

        //When ZuccBot is added to a guild
        private static async Task Discord_GuildCreated(GuildCreateEventArgs e)
        {
            //Make a greeter
            var embed = new DiscordEmbedBuilder() { Title = "**Hi! It's me ZuccBot! Beep Boop.**", Description = "Please read the list below for useful information.", Color = DiscordColor.CornflowerBlue };

            embed.AddField("*Command Usage*","ZuccBot's command prefix is `>` and help can be found using the `help` command. If you do not know what a command prefix is, it is the character before every command. E.g. In order to type the `help` command, one would type `>help` into chat.", false);

            embed.AddField("*Features*","ZuccBot comes with utility commands and features an RPG. Use the `uHelp` command or the `rpgHelp` command to find information on both of these.", false);

            DiscordEmbed greeter = embed.Build();

            //Send the greeter
            var greeterVar = await e.Guild.GetDefaultChannel().SendMessageAsync("", false, greeter);
            await greeterVar.PinAsync();

            //Badda Bing, Badda Boom
            await Task.CompletedTask;
        }

        private static Task Discord_MemberRemoved(GuildMemberRemoveEventArgs e)
        {
            try
            {
                if (!e.Member.IsBot)
                {
                    
                    string path = $"{Directory.GetCurrentDirectory()}\\ChatLogs\\{e.Guild.Id}\\{e.Guild.GetDefaultChannel().Id}\\";//Path to logs
                    string txtfile = $"{e.Guild.GetDefaultChannel().Name}.txt";//The File we are writing too
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Guild.GetDefaultChannel().Name.ToString() + " | " + "**User Removed** : " + e.Member.Username.ToString();//Write this in the Discord log channel
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Member.Username.ToString() + " | " + "User Removed : " + e.Member.Username.ToString();//Write this in the log

                    WriteLog($"{path}", $"{txtfile}", log);//Task for writing the log

                    //For every Guild ZuccBot is in, check if they have a channel name "log", if they don't then create one and log chat to that channel.
                    foreach (DiscordGuild guild in discord.Guilds.Values)
                    {
                        if (guild == e.Guild)
                        {
                            //channels equals guild.Channels as a list.
                            var channels = guild.Channels.ToList();

                            //If a channel exists named "log", then print chat logs to it, if not create that channel and print logs to it.
                            if (channels.Exists(channel => channel.Name == "log"))
                            {
                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                            else
                            {
                                //If the appropiate channel wasn't found then create a text channel name "log"
                                guild.CreateChannelAsync("log", ChannelType.Text);

                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Task.CompletedTask;
        }

        //Same things as MemberRemoved except this time it records when players join
        private static Task Discord_MemberAdded(GuildMemberAddEventArgs e)
        {
            try
            {
                if (!e.Member.IsBot)
                {
                    string path = $"{Directory.GetCurrentDirectory()}\\ChatLogs\\{e.Guild.Id}\\{e.Guild.GetDefaultChannel().Id}\\";
                    string txtfile = $"{e.Guild.GetDefaultChannel().Name}.txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Guild.GetDefaultChannel().Name.ToString() + " | " + "**User Joined** : " + e.Member.Username.ToString();
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Member.Username.ToString() + " | " + "User Joined : " + e.Member.Username.ToString();

                    WriteLog($"{path}", $"{txtfile}", log);

                    //For every Guild ZuccBot is in, check if they have a channel name "log", if they don't then create one and log chat to that channel.
                    foreach (DiscordGuild guild in discord.Guilds.Values)
                    {
                        if (guild == e.Guild)
                        {
                            //channels equals guild.Channels as a list.
                            var channels = guild.Channels.ToList();

                            //If a channel exists named "log", then print chat logs to it, if not create that channel and print logs to it.
                            if (channels.Exists(channel => channel.Name == "log"))
                            {
                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                            else
                            {
                                //If the appropiate channel wasn't found then create a text channel name "log"
                                guild.CreateChannelAsync("log", ChannelType.Text);

                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Task.CompletedTask;
        }

        //Record deleted messages, same as the other logs except it records when a player deleted a message
        private static Task Discord_MessageDeleted(MessageDeleteEventArgs e)
        {
            try
            {
                if (!e.Message.Author.IsBot && logEnabled == true) 
                {
                    string path = $"{Directory.GetCurrentDirectory()}\\ChatLogs\\{e.Guild.Id}\\{e.Channel.Id}\\";
                    string txtfile = $"{e.Channel.Name}.txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Channel.Name.ToString() + " | " + "**User** : " + e.Message.Author.Username.ToString() + " | " + "**Deleted Message** : " + '"' + e.Message.Content + '"';
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Channel.Name.ToString() + " | " + "User : " + e.Message.Author.Username.ToString() + " | " + "Deleted Message : " + '"' + e.Message.Content + '"';

                    WriteLog(path, txtfile, log);

                    //For every Guild ZuccBot is in, check if they have a channel name "log", if they don't then create one and log chat to that channel.
                    foreach (DiscordGuild guild in discord.Guilds.Values)
                    {
                        if (guild == e.Guild)
                        {
                            //channels equals guild.Channels as a list.
                            var channels = guild.Channels.ToList();

                            //If a channel exists named "log", then print chat logs to it, if not create that channel and print logs to it.
                            if (channels.Exists(channel => channel.Name == "log"))
                            {
                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                            else
                            {
                                //If the appropiate channel wasn't found then create a text channel name "log"
                                guild.CreateChannelAsync("log", ChannelType.Text);

                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Task.CompletedTask;
        }

        //Record deleted messages, same as the other logs except it records when a player sends a message
        private static Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            try
            {
                if (!e.Author.IsBot && logEnabled == true)
                {
                    string path = $"{Directory.GetCurrentDirectory()}\\ChatLogs\\{e.Guild.Id}\\{e.Channel.Id}\\";
                    string txtfile = $"{e.Channel.Name}.txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Channel.Name.ToString() + " | " + "**User** : " + e.Author.Username.ToString() + " | " + "**Message** : " + '"' + e.Message.Content + '"';
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Channel.Name.ToString() + " | " + "User : " + e.Author.Username.ToString() + " | " + "Message : " + '"' + e.Message.Content + '"';

                    WriteLog($"{path}", $"{txtfile}", log);

                    //For every Guild ZuccBot is in, check if they have a channel name "log", if they don't then create one and log chat to that channel.
                    foreach (DiscordGuild guild in discord.Guilds.Values)
                    {
                        if (guild == e.Guild)
                        {
                            //channels equals guild.Channels as a list.
                            var channels = guild.Channels.ToList();

                            //If a channel exists named "log", then print chat logs to it, if not create that channel and print logs to it.
                            if (channels.Exists(channel => channel.Name == "log"))
                            {
                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                            else
                            {
                                //If the appropiate channel wasn't found then create a text channel name "log"
                                guild.CreateChannelAsync("log", ChannelType.Text);

                                //_channel will be equal to the first channel found name "log".
                                var _channel = channels.Find(x => x.Name == "log");

                                //Record logs to this channel
                                _channel.SendMessageAsync($"{msg}");

                                //Stop trying to log this message.
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Task.CompletedTask;
        }

        //**NOTE** You can NOT serialize DiscordEmbedBuilders and cast them as DiscordEmbeds upon deserialization, you MUST serialize DiscordEmbeds as DiscordEmbeds, if you need to serialize a DiscordEmbed then construct it FIRST and then serialize, otherwise deserialization will NOT work.

        //This is some really janky code, you have been warned.
        //Most, if not, all of this code is related to paginated embeds as this is the event that's fired on every reaction
        //Ethan helped me with this part, the main problem I was having was that objects weren't serializing to the JSON properly and I was serializing DiscordEmbedBuilders and trying to cast them as a generic var and then construct a DiscordEmbed with that. (You normally create DiscordEmbeds this way, however this was the exception)
        private static async Task Discord_ReactionAdded(MessageReactionAddEventArgs e)
        {
            //This is gonna read the file thats being called (CharacterConfig.txt)
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPG\\CharacterConfig.txt"))
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
                    if (e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot)
                    {
                        //Switch statements couldn't work due to Discord Emojis not being constant, among other things
                        if (e.Emoji == DiscordEmoji.FromName(discord, ":man:") || e.Emoji == DiscordEmoji.FromName(discord, ":leaves:") || e.Emoji == DiscordEmoji.FromName(discord, ":pick:"))
                        {
                            CombatEntity character = new CombatEntity(Race.Dwarf, Class.Knight, 10, 0, 0, 0, e.User.Username, _items: new List<Item>());
                            character.curHP = character.maxHP;

                            if (e.Emoji == DiscordEmoji.FromName(discord, ":man:"))
                            {
                                character.race = Race.Human;
                                character.constitution += 1;
                            }
                            else
                                if (e.Emoji == DiscordEmoji.FromName(discord, ":leaves:"))
                            {
                                character.race = Race.Elf;
                                character.dexterity += 1;
                            }
                            else
                                if (e.Emoji == DiscordEmoji.FromName(discord, ":pick:"))
                            {
                                character.race = Race.Dwarf;
                                character.strength += 1;
                            }

                            //Console.WriteLine(tcr);

                            await e.Message.DeleteAllReactionsAsync();

                            if (!players.ContainsKey(e.Channel.Guild.Id))
                            {
                                locations.Add(e.Channel.Guild.Id, new List<Location>() { new Location("Level_1", 1, Population.Aggressive, null, new List<Entity>(), new List<Item>()), new Location("Level_2", 2, Population.Aggressive, null, new List<Entity>(), new List<Item>()), new Location("Level_3", 3, Population.Aggressive, null, new List<Entity>(), new List<Item>()) });
                                players.Add(e.Channel.Guild.Id, new Dictionary<DiscordUser, CombatEntity>());
                            }

                            if (!e.User.IsBot && !players[e.Channel.Guild.Id].ContainsKey(e.User))
                            {
                                players[e.Channel.Guild.Id].Add(e.User, character);
                            }


                            var msg = await e.Message.ModifyAsync("", embed[1]);

                            //These emojis should correspond with the choices on the next page (page 2 A.K.A. element 1)
                            await msg.CreateReactionAsync(DiscordEmoji.FromName(discord, ":crossed_swords:"));
                            await msg.CreateReactionAsync(DiscordEmoji.FromName(discord, ":alembic:"));
                            await msg.CreateReactionAsync(DiscordEmoji.FromName(discord, ":dagger:"));

                            //The task was completed successfully and needs to be closed
                            await Task.CompletedTask;
                        }
                        else
                        {
                            await Task.CompletedTask;
                        }
                    }
                    else
                    {
                        await Task.CompletedTask;
                    }
                }
                else
                if (e.Message.Embeds[0].Title == embed[1].Title)
                {
                    if (e.Message.Author.IsCurrent == e.Message.Author.IsCurrent && e.Channel == e.Channel && e.User == e.User && !e.User.IsBot)
                    {
                        if (e.Emoji == DiscordEmoji.FromName(discord, ":crossed_swords:") || e.Emoji == DiscordEmoji.FromName(discord, ":alembic:") || e.Emoji == DiscordEmoji.FromName(discord, ":dagger:"))
                        {
                            if (!e.User.IsBot && players[e.Channel.Guild.Id].ContainsKey(e.User))
                            {
                                if (e.Emoji == DiscordEmoji.FromName(discord, ":crossed_swords:"))
                                {
                                    players[e.Channel.Guild.Id][e.User].@class = Class.Knight;
                                    players[e.Channel.Guild.Id][e.User].strength += 1;
                                }
                                else
                                if (e.Emoji == DiscordEmoji.FromName(discord, ":alembic:"))
                                {
                                    players[e.Channel.Guild.Id][e.User].@class = Class.Mage;
                                    players[e.Channel.Guild.Id][e.User].constitution += 1;
                                }
                                else
                                if (e.Emoji == DiscordEmoji.FromName(discord, ":dagger:"))
                                {
                                    players[e.Channel.Guild.Id][e.User].@class = Class.Rouge;
                                    players[e.Channel.Guild.Id][e.User].dexterity += 1;
                                }

                                players[e.Channel.Guild.Id][e.User].curHP = players[e.Channel.Guild.Id][e.User].maxHP;

                                locations[e.Channel.Guild.Id][0].SpawnEnemies(e.Channel.Guild);
                                locations[e.Channel.Guild.Id][0].entities.Add(players[e.Channel.GuildId][e.User]);

                                await e.Message.DeleteAsync();

                                var confirmation = new DiscordEmbedBuilder() { Title = $"{e.User.Username} has created a new character!", Color = DiscordColor.CornflowerBlue };

                                switch (players[e.Channel.Guild.Id][e.User].@class)
                                {
                                    case Class.Knight:
                                        switch (players[e.Channel.Guild.Id][e.User].race)
                                        {
                                            case Race.Human:
                                                confirmation.Description = $"{e.User.Mention} has created a Human Knight.";
                                                break;
                                            case Race.Dwarf:
                                                confirmation.Description = $"{e.User.Mention} has created a Dwarven Knight.";
                                                break;
                                            case Race.Elf:
                                                confirmation.Description = $"{e.User.Mention} has created an Elven  Knight.";
                                                break;
                                            case Race.Goblin:
                                                break;
                                            case Race.Orc:
                                                break;
                                            case Race.Troll:
                                                break;
                                        }
                                        break;
                                    case Class.Mage:
                                        switch (players[e.Channel.Guild.Id][e.User].race)
                                        {
                                            case Race.Human:
                                                confirmation.Description = $"{e.User.Mention} has created a Human Mage.";
                                                break;
                                            case Race.Dwarf:
                                                confirmation.Description = $"{e.User.Mention} has created a Dwarven Mage.";
                                                break;
                                            case Race.Elf:
                                                confirmation.Description = $"{e.User.Mention} has created an Elf Mage.";
                                                break;
                                            case Race.Goblin:
                                                break;
                                            case Race.Orc:
                                                break;
                                            case Race.Troll:
                                                break;
                                        }
                                        break;
                                    case Class.Rouge:
                                        switch (players[e.Channel.Guild.Id][e.User].race)
                                        {
                                            case Race.Human:
                                                confirmation.Description = $"{e.User.Mention} has created a Human Rouge.";
                                                break;
                                            case Race.Dwarf:
                                                confirmation.Description = $"{e.User.Mention} has created a Dwarven Rouge.";
                                                break;
                                            case Race.Elf:
                                                confirmation.Description = $"{e.User.Mention} has created an Elf Rouge.";
                                                break;
                                            case Race.Goblin:
                                                break;
                                            case Race.Orc:
                                                break;
                                            case Race.Troll:
                                                break;
                                        }
                                        break;
                                }

                                DiscordEmbed createConfirmation = confirmation;

                                await e.Channel.SendMessageAsync("", false, createConfirmation);

                                //The task was completed successfully and no longer needs to be active
                                await Task.CompletedTask;
                            }
                            else
                            {
                                await Task.CompletedTask;
                            }
                        }
                        else
                        {
                            await Task.CompletedTask;
                        }
                    }
                    else
                    {
                        await Task.CompletedTask;
                    }
                }
                else
                {
                    await Task.CompletedTask;
                }
            }
        }

        private static Task WriteLog(string directory, string file, string log)
        {
            //If the directory doesn't exist create it.
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //If the file doesn't exist create it.
            if (!File.Exists(directory + file))
            {
                File.Create(directory + file);
            }

            //Append 'log' to the specified file.
            using (StreamWriter logWriter = File.AppendText(directory + file))
            {
                logWriter.WriteLine(log);
            }
            return Task.CompletedTask;
        }

        //This is some ridiculous bullshit that works only when it's not reading from disc, whoever madethe dsharpplus library was to incompetent to realize that maybe when I want to store a DiscordUser to a file that I don't want to do it as a string, I want to do it as a DiscordUser, and you can't even cast a string as a DiscordUser.
        //TL;DR: Shit's fucked, yo.
        public static async Task rpgInit(bool read = true)
        {
            try
            {
                if (read == true)
                {
                    if (!File.Exists($"{Directory.GetCurrentDirectory()}\\GenericRPG\\locations.txt"))
                    {
                        /*if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\GenericRPG"))
                        {
                            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\GenericRPG");
                        }

                        File.Create($"{Directory.GetCurrentDirectory()}\\GenericRPG\\locations.txt");*/

                        players = new Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>>();
                        locations = new Dictionary<ulong, List<Location>>();
                    }
                    /*else
                    {
                        using (StreamReader rpgStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\locations.txt"))
                        using (JsonTextReader rpgReader = new JsonTextReader(rpgStreamReader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            serializer.NullValueHandling = NullValueHandling.Ignore;

                            Dictionary<ulong, List<Location>> _locations = (Dictionary<ulong, List<Location>>)serializer.Deserialize(rpgReader, typeof(Dictionary<ulong, List<Location>>));

                            if (_locations != null)
                            {
                                locations = _locations;
                            }
                            else
                            {
                                _locations = new Dictionary<ulong, List<Location>>();
                            }
                        }
                    }*/

                    if (!File.Exists($"{Directory.GetCurrentDirectory()}\\GenericRPG\\players.txt"))
                    {
                        /*if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\GenericRPG"))
                        {
                            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\GenericRPG");
                        }*/

                        File.Create($"{Directory.GetCurrentDirectory()}\\GenericRPG\\players.txt");

                        players = new Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>>();
                        locations = new Dictionary<ulong, List<Location>>();
                    }
                    /*else
                    {
                        using (StreamReader rpgStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\players.txt"))
                        using (JsonTextReader rpgReader = new JsonTextReader(rpgStreamReader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            serializer.NullValueHandling = NullValueHandling.Ignore;

                            Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>> _players = (Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>>)serializer.Deserialize(rpgReader, typeof(Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>>));

                            if (_players != null)
                            {
                                players = _players;
                            }
                            else
                            {
                                _players = new Dictionary<ulong, Dictionary<DiscordUser, CombatEntity>>();
                            }
                        }
                    }*/
                }
                /*else
                {
                    using (StreamWriter rpgStreamWriter = new StreamWriter($"{Directory.GetCurrentDirectory()}\\GenericRPG\\locations.txt"))
                    using (JsonTextWriter rpgWriter = new JsonTextWriter(rpgStreamWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        serializer.Serialize(rpgWriter, locations);
                    }

                    using (StreamWriter rpgStreamWriter = new StreamWriter($"{Directory.GetCurrentDirectory()}\\GenericRPG\\players.txt"))
                    using (JsonTextWriter rpgWriter = new JsonTextWriter(rpgStreamWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        serializer.Serialize(rpgWriter, players);
                    }
                }*/
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e);
            }

            await Task.CompletedTask;
        }
    }
}
