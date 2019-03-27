using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using ZuccBot.ZuccRPG.Generic;
using System.Collections;
using DSharpPlus.Net;
using ZuccBot.UI;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using ZuccBot;
using System.Runtime.Serialization.Json;
//using System.Data.SQLite;

namespace ZuccBot.ZuccRPG
{

    public class GenericRPG
    {
        const string commandPrefix = "rpg";//All commands relating to the GenericRPG game are prefixed with rpg 

        internal static ulong messageTrack { get; set; }

        const { "Race", }

        List<Location> locations = new List<Location>();//All the locations in this world.
        List<Race> races = new List<Race>();//All the availible races

        Dictionary<DiscordMember, Entity> users = new Dictionary<DiscordMember, Entity>();//Dictionary<Key, Value> (It's basically a HashMap from Java) that is used to pair up Discord Users with their character

        //**START** initialize the world.
        //Command : rpgCreateWorld
        //Current way to create worlds, this was made to take parameters however I haven't added them yet and may change this to start on startup
        [Command(commandPrefix + "CreateWorld"), Aliases(commandPrefix + "StartWorld", commandPrefix + "startworld", commandPrefix + "start"), RequirePermissions(Permissions.Administrator), Hidden]
        public async Task CreateWorld(CommandContext ctx)
        {
            //***PROCESS STARTED***
            //Tell the user that the world is being created, good for understanding what is currently happening and if the world has even begun generating
            await ctx.RespondAsync($"Started to create a new world...");

            //Typing indicator, indicates to the user that the bot is getting the job done, good for telling if the Bot ran into an error mid-process.
            await ctx.TriggerTypingAsync();
            
            //***LOCATION CREATION CODE***
            //This may be removed in future versions.
            
            //TAVERN CODE
            //Create a new location named "Tavern" and add it to the list of accessible locations.
            Location tavern = new Location("Tavern", null, null);//create "Tavern".
            locations.Add(tavern);//Add "Tavern" to the list of accessible locations.
            
            //MINE CODE
            //Create a new location named "Mine" and add it to the list of accessible locations.
            Location mine = new Location("Mine", null, null);//Create "Mine".
            locations.Add(mine);//Add "Mine" to the list of accessible locations.

            //PAWNSHOP CODE
            //Create a new location named "PawnShop" and add it to the list of accessible locations
            Location pawnShop = new Location("PawnShop", null, null);//Create "PawnShop"
            locations.Add(pawnShop);//Add "PawnShop" to the list of accessible locations

            //DUNGEON CODE
            //Create a location named "Dungeon", populate it with a Goblin and add it to the list of accessible locations
            Entity goblin = new Entity("Goblin", 1, 5, locations[locations.Count - 1]);//Create a Goblin
            List<Entity> dungeonEntities = new List<Entity>();//Create a list of Entities
            dungeonEntities.Add(goblin);//Populate the list we just created with the Goblin
            Location dungeon = new Location("Dungeon", dungeonEntities, null);//Create a new location named "Dungeon" and parse in the list of entities we want in it
            locations.Add(dungeon);//Add the "Dungeon" to the list of accessible locations
            
            //***PROCESS FINISHED***
            //Tell the user that the process has been finished and they are free to play the game
            await ctx.RespondAsync($"Created a new world to explore!");
        }

        //**Create Character**
        //Command : rpgCreateCharacter
        //This function is subject to a lot of change.
        [Command(commandPrefix + "CreateCharacter")]
        public async Task CreateCharacter(CommandContext ctx)
        {
            //Tell the user the proccess has begun.
            await ctx.RespondAsync($"Started creating a character for {ctx.User.Mention}...");

            //This let's us know if the Bot ran into problems during the function (The Bot will stop typing and nothing will happen.)
            await ctx.TriggerTypingAsync();

            //var json = @"{""title"":""Race"",""description"":""Select a race."",""fields"":[{""name"":"":man: *Human* "",""value"":""Example Description\nAbility Score: This is __not__ implemented yet."",""inline"":false},{""name"":"":leaves: *Elf*"",""value"":""Example Description\nAbility Score: This is __not__ implemented yet"",""inline"":false},{""name"":"":pick: *Dwarf* "",""value"":""Example Description\nAbility Score: This is __not__ implemented yet"",""inline"":false}]}";

            //var embed = JsonConvert.DeserializeObject<DiscordEmbed>(json);

            DiscordEmbedBuilder embed = JsonConvert.DeserializeObject<DiscordEmbedBuilder>(File.ReadAllText(@"C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.json"));
            Console.WriteLine(embed.ToString());
            await ctx.Channel.SendMessageAsync($"", false, embed);

            /*using (StreamReader r = File.OpenText("C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.json"))
            {
                Console.WriteLine(0);
                JsonSerializer serializer = new JsonSerializer();
                Console.WriteLine(1);
                var temp = (DiscordEmbedBuilder)serializer.Deserialize(r, typeof(DiscordEmbedBuilder));
                var embed = new DiscordEmbedBuilder();
                Console.WriteLine(2);
                Console.WriteLine(temp.ToString());
                JsonConvert.PopulateObject(temp.ToString(), embed);
                Console.WriteLine(3);
                await ctx.Channel.SendMessageAsync($"", false, embed);
            }*/

            /*string filepath = "C:\\Users\\scar4\\Desktop\\Repositories\\ZuccBot\\ZuccBot\\ZuccBot\\ZuccRPG\\CharacterConfig.json";
            string result = string.Empty;
            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                result = jobj.ToString();
                Console.WriteLine(result);
            }*/
            //var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(filepath);

            //var msg = await ctx.Channel.SendMessageAsync($"", false, embed);
            //await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":man:"));
            //await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":leaves:"));
            //await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pick:"));
        }

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
        [Command(commandPrefix + "Attack"), Aliases("attack", "fight", "hit", "Fight", "Hit")]
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
                                if (_entity.name == name)
                                {
                                    //**PLAYER OFFENDER, ENTITY DEFENDER**
                                    //Alert that displays an offending party and defending party.
                                    await ctx.RespondAsync($"{ctx.User.Mention} is attacking `{_entity.name}` in `{users[ctx.Member].location.name}`.");
                                    await ctx.TriggerTypingAsync();

                                    //Reduce the defending party's health value by the offending party's damage value
                                    _entity.health -= users[ctx.Member].damage;

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
        }
    }
}
