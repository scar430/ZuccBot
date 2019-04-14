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
    public class Program
    {
        public static DiscordClient discord;
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
                Token = "NTUzNDYwMTEwMzI5MTE4NzIx.XK-wtg.2Q81ZcxriDHN-U1oTiGdiPaBucY",

                //You know the drill...
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                //What could this be UWU
                //I chose the period because it's easy to select on a phone and also one of the easiest buttons to reach from the bottom right side of the keyboard
                StringPrefix = "."
            });

            //This basically detects reactions, it's subscribing to a Task called 'Discord_ReactionAdded', it is listed below...
            discord.MessageReactionAdded += Discord_ReactionAdded;
            discord.MessageCreated += Discord_MessageCreated;
            discord.MessageDeleted += Discord_MessageDeleted;
            discord.GuildMemberAdded += Discord_MemberAdded;
            discord.GuildMemberRemoved += Discord_MemberRemoved;

            //Command subscriptions
            commands.RegisterCommands<Commands>();//General commands (banning, kicking, etc.)
            commands.RegisterCommands<GenericRPG>();//RPG commands (Create characters, attack entities, etc.)

            await discord.ConnectAsync();//Is anyone listening, am I all alone?
            await Task.Delay(-1);//Wait infinitely. Bot purgatory. >:)
        }

        //Logs of players being removed and added are attributed to the default channel

        private static Task Discord_MemberRemoved(GuildMemberRemoveEventArgs e)
        {
            try
            {
                if (!e.Member.IsBot)
                {
                    string path = (Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Guild.GetDefaultChannel().Name.ToString());
                    string txtfile = "\\" + e.Guild.GetDefaultChannel().Name.ToString() + ".txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Guild.GetDefaultChannel().Name.ToString() + " | " + "**User Removed** : " + e.Member.Username.ToString();
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Member.Username.ToString() + " | " + "User Removed : " + e.Member.Username.ToString();

                    if (System.IO.File.Exists(path) == false)
                    {
                        Directory.CreateDirectory(path);

                        if (Directory.GetFiles(path).Contains(txtfile))
                        {
                            using (StreamWriter fs = File.CreateText(path + txtfile))
                            {
                                //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages and using a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                                using (StreamWriter file = File.AppendText(path + txtfile))
                                {
                                    using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Member.Username.ToString() + "\\" + e.Member.Username.ToString() + ".txt"))
                                    {
                                        file.WriteLine(log);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                            using (StreamWriter file = File.AppendText(path + txtfile))
                            {
                                file.WriteLine(log);
                            }
                        }
                    }
                    else
                    {
                        //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                        using (StreamWriter file = File.AppendText(path))
                        {
                            using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Member.Username.ToString() + "\\" + e.Member.Username.ToString() + ".txt"))
                            {
                                file.WriteLine(msg);
                            }
                        }
                    }

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

        private static Task Discord_MemberAdded(GuildMemberAddEventArgs e)
        {
            try
            {
                if (!e.Member.IsBot)
                {
                    string path = (Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Guild.GetDefaultChannel().Name.ToString());
                    string txtfile = "\\" + e.Guild.GetDefaultChannel().Name.ToString() + ".txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Guild.GetDefaultChannel().Name.ToString() + " | " + "**User Joined** : " + e.Member.Username.ToString();
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Member.Username.ToString() + " | " + "User Joined : " + e.Member.Username.ToString();

                    if (System.IO.File.Exists(path) == false)
                    {
                        Directory.CreateDirectory(path);

                        if (Directory.GetFiles(path).Contains(txtfile))
                        {
                            using (StreamWriter fs = File.CreateText(path + txtfile))
                            {
                                //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                                using (StreamWriter file = File.AppendText(path + txtfile))
                                {
                                    using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Member.Username.ToString() + "\\" + e.Member.Username.ToString() + ".txt"))
                                    {
                                        file.WriteLine(log);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                            using (StreamWriter file = File.AppendText(path + txtfile))
                            {
                                file.WriteLine(log);
                            }
                        }
                    }
                    else
                    {
                        //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                        using (StreamWriter file = File.AppendText(path))
                        {
                            using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Member.Username.ToString() + "\\" + e.Member.Username.ToString() + ".txt"))
                            {
                                file.WriteLine(msg);
                            }
                        }
                    }

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

        private static Task Discord_MessageDeleted(MessageDeleteEventArgs e)
        {
            try
            {
                if (!e.Message.Author.IsBot)
                {
                    string path = (Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Channel.Name.ToString());
                    string txtfile = "\\" + e.Channel.Name.ToString() + ".txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Channel.Name.ToString() + " | " + "**User** : " + e.Message.Author.Username.ToString() + " | " + "**Deleted Message** : " + '"' + e.Message.Content + '"';
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Channel.Name.ToString() + " | " + "User : " + e.Message.Author.Username.ToString() + " | " + "Deleted Message : " + '"' + e.Message.Content + '"';

                    if (System.IO.File.Exists(path) == false)
                    {
                        Directory.CreateDirectory(path);

                        if (Directory.GetFiles(path).Contains(txtfile))
                        {
                            using (StreamWriter fs = File.CreateText(path + txtfile))
                            {
                                //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                                using (StreamWriter file = File.AppendText(path + txtfile))
                                {
                                    using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Channel.Name.ToString() + "\\" + e.Channel.Name.ToString() + ".txt"))
                                    {
                                        file.WriteLine(log);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                            using (StreamWriter file = File.AppendText(path + txtfile))
                            {
                                file.WriteLine(log);
                            }
                        }
                    }
                    else
                    {
                        //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                        using (StreamWriter file = File.AppendText(path))
                        {
                            using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Channel.Name.ToString() + "\\" + e.Channel.Name.ToString() + ".txt"))
                            {
                                file.WriteLine(msg);
                            }
                        }
                    }

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

        private static Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            try
            {
                if (!e.Author.IsBot)
                {
                    string path = (Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Channel.Name.ToString());
                    string txtfile = "\\" + e.Channel.Name.ToString() + ".txt";
                    string msg = "**Date** : " + DateTime.Now.ToString() + " | " + "**Guild** : " + e.Guild.Name.ToString() + " | " + "**Channel** : " + e.Channel.Name.ToString() + " | " + "**User** : " + e.Author.Username.ToString() + " | " + "**Message** : " + '"' + e.Message.Content + '"';
                    string log = "Date : " + DateTime.Now.ToString() + " | " + "Guild : " + e.Guild.Name.ToString() + " | " + "Channel : " + e.Channel.Name.ToString() + " | " + "User : " + e.Author.Username.ToString() + " | " + "Message : " + '"' + e.Message.Content + '"';

                    if (System.IO.File.Exists(path) == false)
                    {
                        Directory.CreateDirectory(path);

                        if (Directory.GetFiles(path).Contains(txtfile))
                        {
                            using (StreamWriter streamWriter = File.CreateText(path + txtfile))
                            {
                                //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                                using (StreamWriter file = File.AppendText(path + txtfile))
                                {
                                    using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Channel.Name.ToString() + "\\" + e.Channel.Name.ToString() + ".txt"))
                                    {
                                        file.WriteLine(log);
                                    }
                                }
                            }
                            /*
                            foreach (DiscordAttachment attachment in e.Message.Attachments)
                            {
                                attachment.
                                using (FileStream stream = File.Create(path + attachment.GetType().Name.ToString()))
                                {
                                    
                                }
                            }*/
                        }
                        else
                        {
                            //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                            using (StreamWriter file = File.AppendText(path + txtfile))
                            {
                                file.WriteLine(log);
                            }
                        }
                    }
                    else
                    {
                        //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
                        using (StreamWriter file = File.AppendText(path))
                        {
                            using (StreamWriter _file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + e.Guild.Name.ToString() + "\\" + e.Channel.Name.ToString() + "\\" + e.Channel.Name.ToString() + ".txt"))
                            {
                                file.WriteLine(msg);
                            }
                        }
                    }

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