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

namespace ZuccBot.ZuccRPG
{
    public class GenericRPG
    {
        const string commandPrefix = "rpg";//All commands relating to the GenericRPG game are prefixed with rpg, this helps stop command clutter

        //These two lists may be temporary
        List<Location> locations = new List<Location>();//All the locations in this world.

        Dictionary<DiscordMember, Entity> users = new Dictionary<DiscordMember, Entity>();//Dictionary<Key, Value> (It's basically a HashMap from Java) that is used to pair up Discord Users with their character

        //**Create Character**
        //Command : rpgCreateCharacter
        //This function is subject to a lot of change.
        [Command(commandPrefix + "CreateCharacter"), Aliases(commandPrefix + "cc", commandPrefix + "createCharacter", commandPrefix + "createcharacter", commandPrefix + "newcharacter", commandPrefix + "newchar", commandPrefix + "newCharacter", commandPrefix + "createchar")]
        public async Task CreateCharacter(CommandContext ctx)
        {
            // serialize JSON directly to a file
            /*using (StreamWriter file = File.CreateText(@"C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.txt"))
            {
                var embed = new DiscordEmbedBuilder() { Title = "Race", Description = "Select the reaction the corresponds with the Emoji associated with your desired selection.", Color = DiscordColor.CornflowerBlue };
                embed.AddField(":man: __Human__", "Example Description\nAbility Score: This is __not__ implemented yet.");
                embed.AddField(":leaves: __Elf__", "Example Description\nAbility Score: This is __not__ implemented yet.");
                embed.AddField(":pick: __Dwarf__", "Example Description\nAbility Score: This is __not__ implemented yet.");

                List<DiscordEmbedBuilder> embeds = new List<DiscordEmbedBuilder>();
                embeds.Add(embed);

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, embeds);
            }*/

            //Read "CharacterConfig.txt"
            //C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.txt
            using (StreamReader file = File.OpenText(@"C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.txt"))
            {
                //More of a debug feature, jsut checks what your doing and can log what your accessing
                ITraceWriter tcr = new MemoryTraceWriter();

                //Is gonna use JSON magic on whatever we are targeting with the current file stream
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                //print the memory tracer
                Console.WriteLine(tcr);

                //Create the first page in the character creation embed, call the first embed list in the character creation config and give it it's reactions that correspond with it'selections
                List<DiscordEmbed> embed = (List<DiscordEmbed>)serializer.Deserialize(file, typeof(List<DiscordEmbed>));
                var msg = await ctx.Channel.SendMessageAsync($"", false, embed[0]);
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":man:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":leaves:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pick:"));
            }
        }

        //**NOTE** This WILL change
        //**LIST PLAYERS**
        //Command : rpgPlayers
        //Lists all of the players who are partaking in the rpg (list of players is limited to server.)
        [Command(commandPrefix + "Players")]
        public async Task Players(CommandContext ctx)
        {
            //For every member who has created an avatar...
            foreach (DiscordMember _member in users.Keys)
            {
                //List the name of the current iterated player
                await ctx.RespondAsync($"{_member.DisplayName}\n");
            }
            
        }

        //**TRAVEL**
        //Command : rpgGoTo specifiedLocation
        [Command(commandPrefix + "GoTo"), Aliases(commandPrefix + "goto", commandPrefix + "go", commandPrefix + "to")]
        public async Task GoTo(CommandContext ctx, string location)
        {
            //Try moving the players character, if there is an exception catch it.
            try
            {
                //Look through every location...
                foreach (Location _location in locations)
                {
                    //If the current iterated location is equal to the specified one...
                    if (location == _location.name)
                    {
                        //Changed the users location to the specified one.
                        users[ctx.Member].location = _location;
                        
                        //Inform the user that their avatar was moved to another location
                        await ctx.RespondAsync($"{ctx.User.Mention}'s avatar was moved to `{_location.name}`");
                        
                        //break since the loop no longer needs to continue
                        break;
                    }
                }
            }
            catch
            {
                //If an exception was thrown it asks if the player created a character
                await ctx.RespondAsync($"A serious problem has occurred. {ctx.User.Mention} may __NOT__ have created a character or the location specified was null.");
            }
            
        }

        [Command(commandPrefix + "LocationInformation"), Aliases(commandPrefix + "locInfo", commandPrefix + "hereinfo", commandPrefix + "infohere", commandPrefix + "infoHere", commandPrefix + "LocInfo")]
        public async Task LocationInformation(CommandContext ctx)
        {
            try
            {
                //Being the list by stating what location this information pertains to and list it's name.
                await ctx.RespondAsync($"Information on `{users[ctx.Member].location.name}`: \n**Name** : \n`{users[ctx.Member].location.name}`\n");

                //If there are entities in the location...
                if (users[ctx.Member].location.entities != null && users[ctx.Member].location.entities.Count > 0)
                {
                    //Begin the list of entities by stating it is entities
                    await ctx.RespondAsync($"**Entities** : \n");

                    //Look through all the locations
                    foreach (Location _location in locations)
                    {
                        //If the location is the one the user is in...
                        if (_location.name == users[ctx.Member].location.name)
                        {
                            //For every entity at the location...
                            foreach (Entity _entity in users[ctx.Member].location.entities)
                            {
                                //Print out the entity's name
                                await ctx.RespondAsync($"`{_entity.name}`\n");
                            }
                        }
                    }
                }
            }
            catch
            {
                //If an exception was thrown, the player probably didn't create a character
                await ctx.RespondAsync($"A serious problem has occurred. The user has probably __NOT__ created a character yet.");
            }
        }

        //**LIST LOCATIONS**
        //Command : rpgWhatLocations
        //This is usually used to find locations to travel to with 'rpgGoTo'
        [Command(commandPrefix + "WhatLocations")]
        public async Task WhatLocations(CommandContext ctx)
        {
            //The Bot announces it is listing something
            await ctx.RespondAsync($"Here's a list of locations for {ctx.User.Mention} : ");
            for (int i = 0; i < (locations.Count); i++)
            {
                //For every accessible location there is, print it's name
                await ctx.RespondAsync($"\n`{locations[i].name}`");
            }
        }

        //**NOTE** This command is subject to a lot of change
        //**ATTACK**
        //Command : rpgAttack
        //Used to attack specified entities in the players current location
        /*[Command(commandPrefix + "Attack"), Aliases("attack", "fight", "hit", "Fight", "Hit")]
        public async Task Attack(CommandContext ctx, string name)
        {
            //In case of exceptions...
            try
            {
                if (users[ctx.Member].location.entities != null && users[ctx.Member].location.entities.Count > 0)
                {
                    foreach (Location _location in locations)
                    {
                        if (_location.name == users[ctx.Member].location.name)
                        {
                            foreach (Entity _entity in users[ctx.Member].location.entities)
                            {
                                if (_entity is CombatEntity)
                                {
                                    if (_entity.name == name)
                                    {
                                        CombatEntity enemy = (CombatEntity)_entity;
                                        CombatEntity player = (CombatEntity)users[ctx.Member];

                                        //**PLAYER OFFENDER, ENTITY DEFENDER**
                                        //Alert that displays an offending party and defending party.
                                        await ctx.RespondAsync($"{ctx.User.Mention} is attacking `{_entity.name}` in `{users[ctx.Member].location.name}`.");
                                        await ctx.TriggerTypingAsync();

                                        //Reduce the defending party's health value by the offending party's damage value
                                        enemy.curHP -= player.;

                                        if (_entity.health <= 0)
                                        {
                                            //Alert that the offending party has slain the defending party.
                                            await ctx.RespondAsync($"{ctx.User.Mention} has killed `{_entity.name}` at `{users[ctx.Member].location.name}`.");
                                            _location.entities.Remove(_entity);
                                            break;
                                        }
                                        else
                                        {
                                            //Alert that the offending party has struck the defending party.
                                            await ctx.RespondAsync($"{ctx.User.Mention} has attacked `{_entity.name}` for `{users[ctx.Member].damage}` damage at `{users[ctx.Member].location.name}`.");
                                        }

                                        //**ENTITY OFFENDER, PLAYER DEFENDER**

                                        //Alert that an entity is attacking the member who initiated the command at a location
                                        await ctx.RespondAsync($"`{_entity.name}` is attacking {ctx.Member.Mention} in `{users[ctx.Member].location.name}`.");
                                        await ctx.TriggerTypingAsync();

                                        //Reduce the defending party's health value by the offending party's damage value
                                        users[ctx.Member].health -= _entity.damage;

                                        //if statement to check for death or just health reduction
                                        if (users[ctx.Member].health <= 0)
                                        {
                                            //Alert that the offending party has slain the defending party.
                                            await ctx.RespondAsync($"`{_entity.name}` has killed {ctx.Member.Mention} at `{users[ctx.Member].location.name}`.");
                                        }
                                        else
                                        {
                                            //Alert that the offending party has struck the defending party.
                                            await ctx.RespondAsync($"`{_entity.name}` has attacked {ctx.Member.Mention} for `{_entity.damage}` damage at `{users[ctx.Member].location.name}`.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    await ctx.RespondAsync($"There are no entities to attack in `{users[ctx.Member].location.name}`.");
                }
            }
            catch
            {
                //As of now (March 12 2019), I'm not sure why this would be thrown as I have never had it happen, and there's nothing (I know of) that could cause it. If you do get this exception, Good Luck!
                await ctx.RespondAsync($"An unknown error has occurred.");
            }
        }*/
    }
}
