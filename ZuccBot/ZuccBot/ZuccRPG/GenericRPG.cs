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
using ZuccBot.ZuccRPG.RPGassets;
using System.Data.SQLite;

namespace ZuccBot.ZuccRPG
{
    //Combat works as follows: (Defender Health + Armor Die + Skill Score) - (Weapon Die + Skill Score) = new Defender Health

    public class GenericRPG
    {
        const string commandPrefix = "rpg>";//All commands relating to the GenericRPG game are prefixed with rpg, this helps stop command clutter

        //These two lists may be temporary
        public List<Location> locations;//All the locations in this world.

        //**Create Character**
        //Command : rpgCreateCharacter
        //This function is subject to a lot of change.
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
            }*/

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
            }

            //Read "CharacterConfig.txt"
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\CharacterConfig.txt"))
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
            await ctx.Message.DeleteAsync();
            await Task.CompletedTask;
        }

        //**LIST PLAYERS**
        //Command : rpgPlayers
        //Lists all of the players who are partaking in the rpg (list of players is limited to server.)
        [Command(commandPrefix + "players")]
        public async Task Players(CommandContext ctx)
        {
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\PlayersConfig.txt"))
            {
                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//We're gonna use this bad boy to read files from the current 

                Dictionary<DiscordUser, CombatEntity> characters = (Dictionary<DiscordUser, CombatEntity>)serializer.Deserialize(file, typeof(Dictionary<DiscordUser, CombatEntity>));
                //For every member who has created an avatar...
                foreach (DiscordUser user in characters.Keys)
                {
                    //List the name of the current iterated player
                    await ctx.RespondAsync($"{user.Username}\n");
                }
            }
            await ctx.Message.DeleteAsync();
        }

        //**TRAVEL**
        //Command : rpgGoTo specifiedLocation
        [Command(commandPrefix + "go")]
        public async Task GoTo(CommandContext ctx, string location)
        {
            //Try moving the players character, if there is an exception catch it.
            try
            {
                using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\LocationConfig.txt"))
                {
                    if (locations == null)
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
                }
                //Look through every location...
                foreach (Location _location in locations)
                {
                    //If the current iterated location is equal to the specified one...
                    if (location == _location.name)
                    {
                        //Changed the users location to the specified one.
                        //users[ctx.Member].location = _location;
                        
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
            await ctx.Message.DeleteAsync();
        }

        [Command(commandPrefix + "here")]
        public async Task LocationInformation(CommandContext ctx)
        {
            try
            {
                using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\LocationConfig.txt"))
                {
                    if (locations == null)
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
                }
                foreach (Location location in locations)
                {
                    using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\PlayersConfig.txt"))
                    {
                        ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                        JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });//We're gonna use this bad boy to read files from the current 

                        Dictionary<DiscordUser, CombatEntity> characters = (Dictionary<DiscordUser, CombatEntity>)serializer.Deserialize(file, typeof(Dictionary<DiscordUser, CombatEntity>));

                        if (location.entities.ContainsValue(characters[ctx.User]))
                        {
                            //Being the list by stating what location this information pertains to and list it's name.
                            await ctx.RespondAsync($"Information on `{location.name}`: \n**Name** : \n`{location.name}`\n");

                            //If there are entities in the location...
                            if (location.entities != null && location.entities.Count > 0)
                            {
                                //Begin the list of entities by stating it is entities
                                await ctx.RespondAsync($"**Entities** : \n");

                                //For every entity at the location...
                                foreach (Entity _entity in location.entities.Values)
                                {
                                    //Print out the entity's name
                                    await ctx.RespondAsync($"`{_entity.name}`\n");
                                }
                            }
                            break;
                        }
                        else
                        {
                            foreach (Location subLocation in location.levels)
                            {
                                if (subLocation.entities.ContainsValue(characters[ctx.User]))
                                {
                                    //Being the list by stating what location this information pertains to and list it's name.
                                    await ctx.RespondAsync($"Information on `{subLocation.name}`, located in `{location.name}`: \n**Name** : \n`{subLocation.name}`\n");

                                    //If there are entities in the location...
                                    if (subLocation.entities != null && subLocation.entities.Count > 0)
                                    {
                                        //Begin the list of entities by stating it is entities
                                        await ctx.RespondAsync($"**Entities** : \n");

                                        //For every entity at the location...
                                        foreach (Entity _entity in subLocation.entities.Values)
                                        {
                                            //Print out the entity's name
                                            await ctx.RespondAsync($"`{_entity.name}`\n");
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
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
        [Command(commandPrefix + "locations")]
        public async Task WhatLocations(CommandContext ctx)
        {
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPGConfig\\LocationConfig.txt"))
            {
                if (locations == null)
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
            }

            //The Bot announces it is listing something
            await ctx.RespondAsync($"Here's a list of locations for {ctx.User.Mention} : ");
            for (int i = 0; i < (locations.Count); i++)
            {
                //For every accessible location there is, print it's name
                await ctx.RespondAsync($"\n`{locations[i].name}`");
            }
            await ctx.Message.DeleteAsync();
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
                await ctx.Message.DeleteAsync();
            }
            catch
            {
                //As of now (March 12 2019), I'm not sure why this would be thrown as I have never had it happen, and there's nothing (I know of) that could cause it. If you do get this exception, Good Luck!
                await ctx.RespondAsync($"An unknown error has occurred.");
            }
        }*/
    }
}
