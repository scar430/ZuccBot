using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using System.Collections;
using DSharpPlus.Net;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using ZuccBot;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Main.ZuccRPG.RPGassets;

namespace ZuccBot.DD
{
    class ddCommands
    {
        const string prefix = "dd";

        //Begin D&D embed
        [Command(prefix), Hidden]
        public async Task start(CommandContext ctx)
        {
            using (StreamWriter file = File.CreateText(@"C:\\Users\\fir1.MY\\Desktop\\ProcessingProjects\\ProcessingGithub\\ZuccBot\\ZuccBot\\ZuccBot\\testConfig.txt"))
            {
                var embed = new DiscordEmbedBuilder() { Title = "Race", Description = "Select the reaction the corresponds with the Emoji associated with your desired selection.", Color = DiscordColor.CornflowerBlue };
                embed.AddField(":man: __Human__", "Example Description\nAbility Score: This is __not__ implemented yet.");
                embed.AddField(":leaves: __Elf__", "Example Description\nAbility Score: This is __not__ implemented yet.");
                embed.AddField(":pick: __Dwarf__", "Example Description\nAbility Score: This is __not__ implemented yet.");

                Dictionary<string, DiscordEmbedBuilder> embeds = new Dictionary<string, DiscordEmbedBuilder>();
                embeds.Add("key", embed);

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, embeds);
            }
            Console.WriteLine("test");
            using (StreamReader file = File.OpenText(@"C:\\Users\\fir1.MY\\Desktop\\ProcessingProjects\\ProcessingGithub\\ZuccBot\\ZuccBot\\ZuccBot\\dd\\ddConfig.txt"))
            {
                //More of a debug feature, jsut checks what your doing and can log what your accessing
                ITraceWriter tcr = new MemoryTraceWriter();

                //Is gonna use JSON magic on whatever we are targeting with the current file stream
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                //print the memory tracer
                Console.WriteLine(tcr);

                //Create the first page in the character creation embed, call the first embed list in the character creation config and give it it's reactions that correspond with it'selections
                Dictionary<string, DiscordEmbed> embed = (Dictionary<string, DiscordEmbed>)serializer.Deserialize(file, typeof(Dictionary<string, DiscordEmbed>));
                var msg = await ctx.Channel.SendMessageAsync($"", false, embed["chapters"]);
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":regional_indicator_k:"));
            }
        }
    }
}
