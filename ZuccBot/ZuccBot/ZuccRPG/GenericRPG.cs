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
using ZuccBot.ZuccRPG;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using ZuccBot.ZuccRPG.RPGassets;
using System.Data.SQLite;

namespace ZuccBot.ZuccRPG
{
    //A lot of locations and features got cut, primarily due me not really knowing what to do caused a lot of quick planning which resulted in constantly having to go back and fix things which means a lot of time wasted. :(

    public class GenericRPG
    {
        const string commandPrefix = "rpg>";//All commands relating to the GenericRPG game are prefixed with rpg, this helps stop command clutter

        //Create a character
        [Command(commandPrefix + "new")]
        public async Task CreateCharacter(CommandContext ctx)
        {
            //Read "CharacterConfig.txt"
            using (StreamReader file = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\CharacterConfig.txt"))
            {
                //More of a debug feature, jsut checks what your doing and can log what your accessing
                ITraceWriter tcr = new MemoryTraceWriter();

                //Is gonna use JSON magic on whatever we are targeting with the current file stream
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                //print the memory tracer
                Console.WriteLine(tcr);

                //Create the first page in the character creation embed, call the first embed list in the character creation config and give it it's reactions that correspond with it'selections
                List<DiscordEmbed> embed = (List<DiscordEmbed>)serializer.Deserialize(file, typeof(List<DiscordEmbed>));
                //var msg = await ctx.Channel.SendMessageAsync($"", false, embed[0]);

                //var dm = await ctx.Member.CreateDmChannelAsync();
                var msg = await ctx.Channel.SendMessageAsync($"", false, embed[0]);

                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":man:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":leaves:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pick:"));
            }
            await ctx.Message.DeleteAsync();
            await Task.CompletedTask;
        }

        //List of all items in the players inventory
        [Command(commandPrefix + "items")]
        public async Task Items(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder() { Title = "**Inventory**", Color = DiscordColor.CornflowerBlue };

            if (Program.players[ctx.Channel.Guild.Id][ctx.User].items.Count > 0)
            {
                foreach (Item item in Program.players[ctx.Channel.Guild.Id][ctx.User].items)
                {
                    embed.AddField(item.name, "No description available.", false);
                }
            }
            else
            {
                embed.Description = $"{ctx.User}'s Inventory is empty.";
            }

            await ctx.RespondAsync("", false, embed);

            await Task.CompletedTask;
        }

        //Equip armor
        [Command(commandPrefix + "equip")]
        public async Task Equip(CommandContext ctx, string itemToEquip)
        {
            var embed = new DiscordEmbedBuilder() { Title = "**Equipment**", Color = DiscordColor.CornflowerBlue };
            //Keep this in memory so we aren't constantly looking for it.
            var vessel = Program.players[ctx.Channel.Guild.Id][ctx.User];

            if (vessel.items.Exists(x => x.name == itemToEquip))
            {
                var item = vessel.items.Find(x => x.name == itemToEquip);
                if (item is Armor)
                {
                    //Remember this as Armor
                    var _item = item as Armor;
                    switch (_item.armor)
                    {
                        case ArmorType.head:
                            vessel.headSlot = _item;
                            break;
                        case ArmorType.torso:
                            vessel.torsoSlot = _item;
                            break;
                        case ArmorType.leg:
                            vessel.legSlot = _item;
                            break;
                        case ArmorType.foot:
                            vessel.footSlot = _item;
                            break;
                    }
                    embed.Description = $"{ctx.User.Mention} has successfully equipped {item.name}.";
                }
                else
                {
                    embed.Description = $"{ctx.User.Mention} is struggling to find their armor.";
                }

                if (vessel.headSlot != null)
                {
                    embed.AddField($"*{vessel.headSlot.name}*", $"Head Slot", false);
                }
                else
                {
                    embed.AddField($"*Empty*", $"Head Slot", false);
                }

                if (vessel.torsoSlot != null)
                {
                    embed.AddField($"*{vessel.torsoSlot}*", $"Torso Slot", false);
                }
                else
                {
                    embed.AddField($"*Empty*", $"Head Slot", false);
                }

                if (vessel.legSlot != null)
                {
                    embed.AddField($"*{vessel.legSlot}*", $"Leg Slot", false);
                }
                else
                {
                    embed.AddField($"*Empty*", $"Head Slot", false);
                }

                if (vessel.footSlot != null)
                {
                    embed.AddField($"*{vessel.footSlot}*", $"Foot Slot", false);
                }
                else
                {
                    embed.AddField($"*Empty*", $"Head Slot", false);
                }
            }
            await Task.CompletedTask;
        }

        //All players in a guild that are partaking in the RPG
        [Command(commandPrefix + "players")]
        public async Task Players(CommandContext ctx)
        {
            
            DiscordEmbedBuilder msg = new DiscordEmbedBuilder() { Title = "**RPG Players**", Description = "A list of all known players in the RPG.", Color = DiscordColor.CornflowerBlue };
            //For every member who has created an avatar...
            foreach (DiscordUser user in Program.players[ctx.Channel.Guild.Id].Keys)
            {
                //List the name of the current iterated player
                msg.AddField($"*{user.Username}*\n", "Known RPG player.", false);
            }

            DiscordEmbed embed = msg;

            await ctx.RespondAsync("", false, embed);
            await ctx.Message.DeleteAsync();
        }

        //travel to a location
        [Command(commandPrefix + "go")]
        public async Task GoTo(CommandContext ctx, string desiredLoc)
        {
            //Try moving the players character, if there is an exception catch it.
            try
            {
                var vessel = Program.players[ctx.Channel.Guild.Id][ctx.User];

                //Look through every location since we need to find where the player is.
                foreach (Location curLoc in Program.locations[ctx.Guild.Id])
                {
                    //Check if the current location has the player in it.
                    if (curLoc.entities.Exists(x => x == vessel as Entity))
                    {
                        //If the location exists then remove them from the old location and place them in the new location.
                        if (Program.locations[ctx.Guild.Id].Exists(x => x.name == desiredLoc))
                        {
                            var newLocation = Program.locations[ctx.Guild.Id].Find(x => x.name == desiredLoc);

                            curLoc.entities.Remove(vessel);
                            newLocation.SpawnEnemies(ctx.Guild);
                            newLocation.entities.Add(vessel);

                            await ctx.Channel.SendMessageAsync("", false, new DiscordEmbedBuilder() { Title = $"{ctx.User.Username} has moved to {newLocation.name}.", Color = DiscordColor.CornflowerBlue });
                        }
                        else
                        {
                            await ctx.Channel.SendMessageAsync("", false, new DiscordEmbedBuilder() { Title = $"{ctx.User.Username} is aimlessly wandering around.", Color = DiscordColor.CornflowerBlue });
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch
            {
                var embed = new DiscordEmbedBuilder() { Title = "**ERROR**", Description = "An error has occurred, either the player has not created a character yet or something is wrong with the Guild's saved data.", Color = DiscordColor.CornflowerBlue};

                DiscordEmbed errorMsg = embed;
                await ctx.RespondAsync("", false, errorMsg);
            }
            await ctx.Message.DeleteAsync();
        }

        //List of all locations
        [Command(commandPrefix + "locations")]
        public async Task WhatLocations(CommandContext ctx)
        {
            string locationString = "";

            foreach (Location location in Program.locations[ctx.Guild.Id])
            {
                locationString += $"\n{location.name}";
            }

            await ctx.RespondAsync("", false, new DiscordEmbedBuilder() { Title = "**Locations**", Description = locationString, Color = DiscordColor.CornflowerBlue });

            await ctx.Message.DeleteAsync();

            await Task.CompletedTask;
        }

        //Get information on the players current location
        [Command(commandPrefix + "here")]
        public async Task here(CommandContext ctx)
        {
            var vessel = Program.players[ctx.Channel.Guild.Id][ctx.User];

            //Look through all locations and check their list of entities to see if it contains the player.
            foreach (Location location in Program.locations[ctx.Guild.Id])
            {
                //Check if the player is in this location.
                if (location.entities.Contains(vessel as Entity))
                {
                    var infoHere = new DiscordEmbedBuilder() { Title = $"**{location.name} Information**", Description = $"Every important detail about a location.", Color = DiscordColor.CornflowerBlue };

                    infoHere.AddField("*Location Name*", $"{location.name}");

                    if (location.entities != null && location.entities.Count > 0)
                    {
                        string entities = "";

                        foreach (Entity entity in location.entities)
                        {
                            entities += $"{entity.name}\n";
                        }

                        infoHere.AddField("*Entities*", entities);
                    }

                    await ctx.RespondAsync("", false, infoHere);
                }
            }

            await ctx.Message.DeleteAsync();
            await Task.CompletedTask;
        }

        //Used to attack specified entities in the players current location
        [Command(commandPrefix + "attack"), Description("Attack an enemy in your current location, e.g. `>rpg>attack weaponName enemyName`")]
        public async Task Attack(CommandContext ctx, string weaponName, string enemyName)
        {
            try
            {
                //Save the player to memory so we aren't constantly looking for it.
                var vessel = Program.players[ctx.Channel.Guild.Id][ctx.User];

                //Look through all locations and check their list of entities to see if it contains the player.
                foreach (Location location in Program.locations[ctx.Guild.Id])
                {
                    //Check if the player is in this location.
                    if (location.entities.Contains(vessel as Entity))
                    {
                        //Create an embed, this is the beginning of the battle message.
                        var embed = new DiscordEmbedBuilder() { Title = $"**Battle**", Color = DiscordColor.CornflowerBlue };

                        //Check that the weapon the player specified exists and is a weapon.
                        if (vessel.items.Exists(x => x.name == weaponName && x is Weapon))
                        {
                            //Save the weapon to memory so we aren't constantly calling it. (Linq operations tend to be heavy)
                            var weapon = vessel.items.Find(x => x.name == weaponName && x is Weapon) as Weapon;

                            //Check that the enemy specified by the player exists in the same location as the player.
                            if (location.entities.Exists(x => x.name == enemyName))
                            {

                                //Save the enemy to memory.  (Linq operations tend to be heavy)
                                var enemy = location.entities.Find(x => x.name == enemyName);

                                //Since the weapon and enemy exist a successful attack message is created.
                                embed.Description = $"Battle between { vessel.name} and { enemyName}.";
                                embed.AddField($"*{vessel.name} has attacked {enemy.name}.*", $"{weapon.Attack(vessel as CombatEntity, enemy as CombatEntity, location, ctx.Guild)}");
                            }
                            else
                            {
                                //Since the enemy couldn't be found create a unsuccessful attack message.
                                embed.Description = $"{vessel.name} is wildly flailing around in {location.name}.";
                            }
                        }
                        else
                        {
                            //Since the weapon couldn't be found create a unsuccessful attack message.
                            embed.Description = $"{vessel.name} is frantically searching their pack for a weapon.";
                        }

                        DiscordEmbed battleMsg = embed;

                        await ctx.RespondAsync("", false, battleMsg);

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                await ctx.Message.DeleteAsync();
                await Task.CompletedTask;
            }
            catch
            {

                await Task.CompletedTask;
            }
        }
    }
}
