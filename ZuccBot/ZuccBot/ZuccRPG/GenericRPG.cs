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
    //Combat works as follows: (Defender Health + Armor Die + Skill Score) - (Weapon Die + Skill Score) = new Defender Health

    public class GenericRPG
    {
        const string commandPrefix = "rpg>";//All commands relating to the GenericRPG game are prefixed with rpg, this helps stop command clutter

        //Create a character
        [Command(commandPrefix + "new")]
        public async Task CreateCharacter(CommandContext ctx)
        {
            /*using (FileStream file = File.OpenWrite(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\LocationConfig.txt"))
            {
                //More of a debug feature, jsut checks what your doing and can log what your accessing
                ITraceWriter tcr = new MemoryTraceWriter();

                //Is gonna use JSON magic on whatever we are targeting with the current file stream
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                JsonConvert.SerializeObject(locations);
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\ItemConfig.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                Weapon club = new Weapon(Die.d4, EquipmentSkill.simple, "Club");
                Weapon shortsword = new Weapon(Die.d4, EquipmentSkill.strength, "Sword");
                Weapon dagger = new Weapon(Die.d4, EquipmentSkill.dextrious, "Dagger");
                Weapon wand = new Weapon(Die.d4, EquipmentSkill.constitution, "Wand");

                Armor leg = new Armor(Die.d4, ArmorType.leg, EquipmentSkill.strength, "Leggings");
                Armor feet = new Armor(Die.d4, ArmorType.foot, EquipmentSkill.simple, "Boots");

                Armor head = new Armor(Die.d4, ArmorType.head, EquipmentSkill.strength, "Helment");
                Armor torso = new Armor(Die.d4, ArmorType.torso, EquipmentSkill.strength, "Chest Plate");

                Armor headd = new Armor(Die.d4, ArmorType.head, EquipmentSkill.dextrious, "Hood");
                Armor torsod = new Armor(Die.d4, ArmorType.torso, EquipmentSkill.dextrious, "Vest");

                Armor headc = new Armor(Die.d4, ArmorType.head, EquipmentSkill.constitution, "Hat");
                Armor torsoc = new Armor(Die.d4, ArmorType.torso, EquipmentSkill.constitution, "Robe");

                Weapon club1 = new Weapon(Die.d6, EquipmentSkill.simple, "Club");
                Weapon shortsword1 = new Weapon(Die.d6, EquipmentSkill.strength, "Sword");
                Weapon dagger1 = new Weapon(Die.d6, EquipmentSkill.dextrious, "Dagger");
                Weapon wand1 = new Weapon(Die.d6, EquipmentSkill.constitution, "Wand");

                Armor leg1 = new Armor(Die.d6, ArmorType.leg, EquipmentSkill.strength, "Leggings");
                Armor feet1 = new Armor(Die.d6, ArmorType.foot, EquipmentSkill.simple, "Boots");

                Armor head1 = new Armor(Die.d6, ArmorType.head, EquipmentSkill.strength, "Helment");
                Armor torso1 = new Armor(Die.d6, ArmorType.torso, EquipmentSkill.strength, "Chest Plate");

                Armor headd1 = new Armor(Die.d6, ArmorType.head, EquipmentSkill.dextrious, "Hood");
                Armor torsod1 = new Armor(Die.d6, ArmorType.torso, EquipmentSkill.dextrious, "Vest");

                Armor headc1 = new Armor(Die.d6, ArmorType.head, EquipmentSkill.constitution, "Hat");
                Armor torsoc1 = new Armor(Die.d6, ArmorType.torso, EquipmentSkill.constitution, "Robe");

                Weapon club2 = new Weapon(Die.d8, EquipmentSkill.simple, "Club");
                Weapon shortsword2 = new Weapon(Die.d8, EquipmentSkill.strength, "Sword");
                Weapon dagger2 = new Weapon(Die.d8, EquipmentSkill.dextrious, "Dagger");
                Weapon wand2 = new Weapon(Die.d8, EquipmentSkill.constitution, "Wand");

                Armor leg2 = new Armor(Die.d8, ArmorType.leg, EquipmentSkill.strength, "Leggings");
                Armor feet2 = new Armor(Die.d8, ArmorType.foot, EquipmentSkill.simple, "Boots");

                Armor head2 = new Armor(Die.d8, ArmorType.head, EquipmentSkill.strength, "Helment");
                Armor torso2 = new Armor(Die.d8, ArmorType.torso, EquipmentSkill.strength, "Chest Plate");

                Armor headd2 = new Armor(Die.d8, ArmorType.head, EquipmentSkill.dextrious, "Hood");
                Armor torsod2 = new Armor(Die.d8, ArmorType.torso, EquipmentSkill.dextrious, "Vest");

                Armor headc2 = new Armor(Die.d8, ArmorType.head, EquipmentSkill.constitution, "Hat");
                Armor torsoc2 = new Armor(Die.d8, ArmorType.torso, EquipmentSkill.constitution, "Robe");

                Weapon club3 = new Weapon(Die.d10, EquipmentSkill.simple, "Club");
                Weapon shortsword3 = new Weapon(Die.d10, EquipmentSkill.strength, "Sword");
                Weapon dagger3 = new Weapon(Die.d10, EquipmentSkill.dextrious, "Dagger");
                Weapon wand3 = new Weapon(Die.d10, EquipmentSkill.constitution, "Wand");

                Armor leg3 = new Armor(Die.d10, ArmorType.leg, EquipmentSkill.strength, "Leggings");
                Armor feet3 = new Armor(Die.d10, ArmorType.foot, EquipmentSkill.simple, "Boots");

                Armor head3 = new Armor(Die.d10, ArmorType.head, EquipmentSkill.strength, "Helment");
                Armor torso3 = new Armor(Die.d10, ArmorType.torso, EquipmentSkill.strength, "Chest Plate");

                Armor headd3 = new Armor(Die.d10, ArmorType.head, EquipmentSkill.dextrious, "Hood");
                Armor torsod3 = new Armor(Die.d10, ArmorType.torso, EquipmentSkill.dextrious, "Vest");

                Armor headc3 = new Armor(Die.d10, ArmorType.head, EquipmentSkill.constitution, "Hat");
                Armor torsoc3 = new Armor(Die.d10, ArmorType.torso, EquipmentSkill.constitution, "Robe");

                Dictionary<string, List<Item>> list = new Dictionary<string, List<Item>>();
                List<Item> list1 = new List<Item>();
                List<Item> list2 = new List<Item>();
                List<Item> list3 = new List<Item>();
                List<Item> list4 = new List<Item>();
                list1.Add(club);
                list1.Add(shortsword);
                list1.Add(dagger);
                list1.Add(wand);
                list1.Add(head);
                list1.Add(torso);
                list1.Add(leg);
                list1.Add(feet);
                list1.Add(headd);
                list1.Add(torsod);
                list1.Add(headc);
                list1.Add(torsoc);
                list2.Add(club1);
                list2.Add(shortsword1);
                list2.Add(dagger1);
                list2.Add(wand1);
                list2.Add(head1);
                list2.Add(torso1);
                list2.Add(leg1);
                list2.Add(feet1);
                list2.Add(headd1);
                list2.Add(torsod1);
                list2.Add(headc1);
                list2.Add(torsoc1);
                list3.Add(club2);
                list3.Add(shortsword2);
                list3.Add(dagger2);
                list3.Add(wand2);
                list3.Add(head2);
                list3.Add(torso2);
                list3.Add(leg2);
                list3.Add(feet2);
                list3.Add(headd2);
                list3.Add(torsod2);
                list3.Add(headc2);
                list3.Add(torsoc2);
                list4.Add(club3);
                list4.Add(shortsword3);
                list4.Add(dagger3);
                list4.Add(wand3);
                list4.Add(head3);
                list4.Add(torso3);
                list4.Add(leg3);
                list4.Add(feet3);
                list4.Add(headd3);
                list4.Add(torsod3);
                list4.Add(headc3);
                list4.Add(torsoc3);

                list.Add("Crude", list1);
                list.Add("Common", list2);
                list.Add("Rare", list3);
                list.Add("Legendary", list4);
                
                serializer.Serialize(writer, list);
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\test.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                Item test = new Item("test");

                List<string> items = new List<string>();
                items.Add("test");

                serializer.Serialize(writer, items);
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\LocationConfig.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                Location Tavern = new Location("Tavern", Population.Passive, _levels: new Location[] { }, _entities: new Dictionary<string[], Entity>(), _items: new List<Item>());

                Location Crypt = new Location("Dungeon", Population.Aggressive, _levels: new Location[] { }, _entities: new Dictionary<string[], Entity>(), _items: new List<Item>());

                List<Location> locations = new List<Location>();
                locations.Add(Tavern);
                locations.Add(Crypt);

                serializer.Serialize(writer, locations);
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\EntityConfig.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                CombatEntity goblin = new CombatEntity(RPGassets.Race.Goblin, Class.Knight, 10, 2, 3, 0, "Goblin", _items: new List<Item>());
                CombatEntity orc = new CombatEntity(RPGassets.Race.Orc, Class.Knight, 25, 3, 2, 1, "Orc", _items: new List<Item>());
                CombatEntity troll = new CombatEntity(RPGassets.Race.Troll, Class.Knight, 50, 5, 2, 0, "Troll", _items: new List<Item>());
                List<CombatEntity> entities = new List<CombatEntity>();
                entities.Add(goblin);
                entities.Add(orc);
                entities.Add(troll);

                serializer.Serialize(writer, entities);
            }

            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\LocationConfig.txt"))
            {
                if(locations == null)
                {
                    //More of a debug feature, jsut checks what your doing and can log what your accessing
                    ITraceWriter tcr = new MemoryTraceWriter();

                    //Is gonna use JSON magic on whatever we are targeting with the current file stream
                    JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                    //print the memory tracer
                    Console.WriteLine(tcr);

                    //Create the first page in the character creation embed, call the first embed list in the character creation config and give it it's reactions that correspond with it'selections
                    locations = (List<Location>)serializer.Deserialize(file, typeof(List<Location>));
                }
            }*/

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
            using (StreamReader file = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Channel.Guild.Id}\\Players.txt"))
            {
                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                Dictionary<string, CombatEntity> players = (Dictionary<string, CombatEntity>)serializer.Deserialize(file, typeof(Dictionary<string, CombatEntity>));

                var embed = new DiscordEmbedBuilder() { Title = "**Inventory**", Description = "All items currently held by the player.", Color = DiscordColor.CornflowerBlue };

                foreach (Item item in players[ctx.User.Username].items)
                {
                    embed.AddField(item.name, "No description available.", false);
                }

                DiscordEmbed items = embed;

                await ctx.RespondAsync("", false, items);
            }

            await Task.CompletedTask;
        }

        //Equip armor
        [Command(commandPrefix + "equip")]
        public async Task Equip(CommandContext ctx, string itemToEquip)
        {
            var embed = new DiscordEmbedBuilder() { Title = "**Equipment**", Color = DiscordColor.CornflowerBlue };
            using (StreamReader file = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Channel.Guild.Id}\\Players.txt"))
            {
                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                Dictionary<string, CombatEntity> players = (Dictionary<string, CombatEntity>)serializer.Deserialize(file, typeof(Dictionary<string, CombatEntity>));

                //Keep this in memory so we aren't constantly looking for it.
                var vessel = players[ctx.User.Username];

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
                        embed.Description = $"You have successfully equipped {item.name}.";
                    }
                    else
                    {
                        embed.Description = $"You have failed to equip {item.name}, this item is not a known piece of equipment.";
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
            }
            await Task.CompletedTask;
        }

        //All players in a guild that are partaking in the RPG
        [Command(commandPrefix + "players")]
        public async Task Players(CommandContext ctx)
        {
            if (File.Exists($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Channel.Guild.Id}\\Players.txt"))
            {
                using (StreamReader file = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Channel.Guild.Id}\\Players.txt"))
                {
                    ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                    JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                    Dictionary<string, CombatEntity> players = (Dictionary<string, CombatEntity>)serializer.Deserialize(file, typeof(Dictionary<string, CombatEntity>));

                    //Create the discordembed that relays who is in the rpg
                    DiscordEmbedBuilder msg = new DiscordEmbedBuilder() { Title = "**RPG Players**", Description = "A list of all known players in the RPG.", Color = DiscordColor.CornflowerBlue };
                    if (players.Keys.Count == 0)
                    {
                        msg.AddField("*No players could be found.*", "Nobody in this guild has started ZuccBot's RPG", false);
                    }
                    else
                    {
                        //For every member who has created an avatar...
                        foreach (string user in players.Keys)
                        {
                            //List the name of the current iterated player
                            msg.AddField($"*{user}*\n","Known RPG player.", false);
                        }
                    }

                    DiscordEmbed embed = msg;

                    await ctx.RespondAsync("", false, embed);
                }
            }
            else
            {
                await ctx.RespondAsync("The file containing the list of players could not be found.");
            }
            await ctx.Message.DeleteAsync();
        }

        //travel to a location
        [Command(commandPrefix + "go")]
        public async Task GoTo(CommandContext ctx, string desiredLoc)
        {
            //Try moving the players character, if there is an exception catch it.
            try
            {
                using (StreamReader playerReader = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Guild.Id}\\Players.txt"))
                using (StreamReader locationReader = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Guild.Id}\\Locations.txt"))
                {
                    ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, ignore it.

                    JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//Create a serializer.

                    //Deserialize dictionary of players.
                    Dictionary<string, CombatEntity> players = (Dictionary<string, CombatEntity>)serializer.Deserialize(playerReader, typeof(Dictionary<string, CombatEntity>));

                    //Deserialize list of locations.
                    List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                    //I don't want to constantly be looking for this, save it to memory.
                    var vessel = players[ctx.User.Username];

                    //Look through every location since we need to find where the player is.
                    foreach (Location curLoc in locations)
                    {
                        //Check if the current location has the player in it.
                        if (curLoc.entities.Exists(x => x == vessel))
                        {
                            //If we found the player then we need to check if the location they want to go to even exists.
                            foreach (Location newLocation in locations)
                            {
                                //If the location exists then remove them from the old location and place them in the new location.
                                if (locations.Exists(x => x.name == desiredLoc))
                                {
                                    curLoc.entities.Remove(vessel);
                                    newLocation.entities.Add(vessel);
                                }
                                else
                                {
                                    continue;
                                }
                                break;
                            }
                            break;
                        }
                        else
                        {
                            continue;
                        }
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
            using (StreamReader file = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPGConfig\\{ctx.Guild.Id}\\Locations.txt"))
            {
                //Tracks what were accessing.
                ITraceWriter tcr = new MemoryTraceWriter();

                //Create a serializer.
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                //Deserialize locations from Locations.txt
                List<Location> locations = (List<Location>)serializer.Deserialize(file, typeof(List<Location>));

                string locationString = "";

                foreach (Location location in locations)
                {
                    locationString += $"\n{location.name}";
                }

                var embed = new DiscordEmbedBuilder() { Title = "*Locations*", Description = locationString, Color = DiscordColor.CornflowerBlue };

                DiscordEmbed locationMsg = embed;

                await ctx.RespondAsync("", false, locationMsg);

                await ctx.Message.DeleteAsync();
            }

            await Task.CompletedTask;
        }

        //Get information on the players current location
        [Command(commandPrefix + "here")]
        public async Task here(CommandContext ctx)
        {
            using (StreamReader locationReader = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Guild.Id}\\Locations.txt"))
            using (StreamReader playerReader = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Guild.Id}\\Players.txt"))
            {
                ITraceWriter tcr = new MemoryTraceWriter();//Acts like a tracker

                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//Create a serializer

                //Deserialize a dictionary of players from Players.txt
                Dictionary<string, CombatEntity> players = (Dictionary<string, CombatEntity>)serializer.Deserialize(playerReader, typeof(Dictionary<string, CombatEntity>));

                //Deserialize a list from Locations.txt
                List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                //Save the player to memory so we aren't constantly looking for it.
                var vessel = players[ctx.User.Username];

                //Look through all locations and check their list of entities to see if it contains the player.
                foreach (Location location in locations)
                {
                    //Check if the player is in this location.
                    if (location.entities.Contains(vessel as Entity))
                    {
                        var embed = new DiscordEmbedBuilder() { Title = $"**{location.name} Information**", Description = $"Every important detail about a location.",Color = DiscordColor.CornflowerBlue};

                        embed.AddField("*Location Name*", $"{location.name}");

                        if (location.entities != null && location.entities.Count > 0)
                        {
                            string entities = "";

                            foreach (Entity entity in location.entities)
                            {
                                entities += $"{entity.name}\n";
                            }

                            embed.AddField("*Entities*", entities);
                        }

                        if (location.levels.Count() > 0)
                        {
                            string subLocations = "";

                            foreach (Location subLocation in location.levels)
                            {
                                subLocations += $"{subLocation.name}\n";
                            }

                            embed.AddField("*Levels*", subLocations);
                        }

                        DiscordEmbed infoHere = embed;

                        await ctx.RespondAsync("", false, infoHere);
                    }
                }

                await ctx.Message.DeleteAsync();
                await Task.CompletedTask;
            }
        }

        //Used to attack specified entities in the players current location
        [Command(commandPrefix + "attack"), Description("Attack an enemy in your current location, e.g. `>rpg>attack weaponName enemyName`")]
        public async Task Attack(CommandContext ctx, string weaponName, string enemyName)
        {
            try
            {
                using (StreamReader locationReader = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Guild.Id}\\Locations.txt"))
                using (StreamReader playerReader = File.OpenText($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{ctx.Guild.Id}\\Players.txt"))
                {
                    ITraceWriter tcr = new MemoryTraceWriter();//Acts like a tracker

                    JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//Create a serializer

                    //Deserialize a dictionary of players from Players.txt
                    Dictionary<string, CombatEntity> players = (Dictionary<string, CombatEntity>)serializer.Deserialize(playerReader, typeof(Dictionary<string, CombatEntity>));

                    //Deserialize a list from Locations.txt
                    List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                    //Save the player to memory so we aren't constantly looking for it.
                    var vessel = players[ctx.User.Username];

                    //Look through all locations and check their list of entities to see if it contains the player.
                    foreach (Location location in locations)
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
                                    embed.AddField($"*{vessel.name} has attacked {enemy.name}.*",$"{weapon.Attack(vessel as CombatEntity, enemy as CombatEntity, location, ctx.Guild)}");
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
