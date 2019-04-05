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

namespace ZuccBot.dd
{
    class ddCommands
    {
        const string prefix = "dd";

        //Begin D&D embed
        [Command(prefix), Hidden]
        public async Task start(CommandContext ctx)
        {
            //C:\\Users\\fir1.MY\\Desktop\\ProcessingProjects\\ProcessingGithub\\ZuccBot\\ZuccBot\\ZuccBot\\dd\\ddConfig.txt
            //C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\dd\\ddConfig.txt
            using (StreamReader file = File.OpenText(@"C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\dd\\ddConfig.txt"))
            {
                //More of a debug feature, jsut checks what your doing and can log what your accessing
                ITraceWriter tcr = new MemoryTraceWriter();

                //Is gonna use JSON magic on whatever we are targeting with the current file stream
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                //print the memory tracer
                Console.WriteLine(tcr);

                //Create the first page in the character creation embed, call the first embed list in the character creation config and give it it's reactions that correspond with it'selections
                Dictionary<string, DiscordEmbed> embed = (Dictionary<string, DiscordEmbed>)serializer.Deserialize(file, typeof(Dictionary<string, DiscordEmbed>));
                var msg = await ctx.Channel.SendMessageAsync($"", false, embed["topics"]);
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":one:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":two:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":three:"));
            }
        }
    }
}
