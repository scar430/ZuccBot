using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Data.SQLite;

namespace Discord.DiscordBots.ZuccBot.Games.GenericRPG
{
    public class GenericRPG
    {
        //***NOTE*** Games are server based so your characters can not interact with games on other servers and can not be carried over to other servers.
        const string commandPrefix = "rpg";
        List<Location> locations = new List<Location>();//All the locations in this world.

        Dictionary<DiscordMember, Entity> users = new Dictionary<DiscordMember, Entity>();//Dictionary<Key, Value> (It's basically a HashMap from Java) that is used to pair up Discord Users with their character

        SQLiteConnection curConnection = new SQLiteConnection("Data Source =:memory:; Version = 3; New = True;");//Create a new connection.

        //**START** initialize the world.
        //Command : rpgCreateWorld
        //Current way to create worlds, this was made to take parameters however I haven't added them yet and may change this to start on startup
        [Command(commandPrefix + "CreateWorld"), Aliases(commandPrefix + "StartWorld", commandPrefix + "startworld", commandPrefix + "start"), RequirePermissions(Permissions.Administrator), Hidden]
        public async Task CreateWorld(CommandContext ctx)
        {
            //***PROCESS STARTED***
            //Tell the user that the world is being created, good for understanding what is currently happening and if the world has even begun generating
            await ctx.RespondAsync($"Started to create a new world...");

            SQLiteConnection.CreateFile(ctx.Guild.Name + "RPGdatabase.sqlite");

            //**NOTE** don't forget to add a close.
            curConnection.Open();//Open the connection, this bad boy is ready to go.

            string sql = "create table names (name string)";

            sql = "insert into names (name) values (" + ctx.Member.Mention + ")";
            SQLiteCommand command = new SQLiteCommand(sql, curConnection);
            command.ExecuteNonQuery();

            sql = "select " + ctx.Member.Mention + " from names order by name desc";
            command = new SQLiteCommand(sql, curConnection);

            SQLiteDataReader reader = command.ExecuteReader();

            sql = "select * from highscores order by score desc";
            command = new SQLiteCommand(sql, curConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"]);

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

            //Create a table to read values from.
            string sql = "create table names (name string)";

            SQLiteCommand command = new SQLiteCommand(sql, curConnection);

            command.ExecuteNonQuery();

            sql = ("insert into names (name) values ( '" + ctx.Member.Mention + "' )");

            command = new SQLiteCommand(sql, curConnection);

            command.ExecuteNonQuery();

            //Create a new Entity
            Entity player = new Entity(ctx.Member.DisplayName, 2, 100, locations[0]);
            users.Add(ctx.Member, player);//Pair this entity with the member who initiated the command into the dicitionary "users"
            await ctx.RespondAsync($"Finished creating a character for {ctx.User.Mention}! \nHappy trails, partner!");//Let's the user know the process is over and was completed successfully and is also a reference :)
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
                        //Look through every member...
                        /*foreach (DiscordMember _member in users.Keys)
                        {
                            //If the current iterated member is equal to the one that initiated the command...
                            if(_member == ctx.Member)
                            {
                                //Changed the users location to the specified one.
                                users[_member].location = _location;
                                
                                //Inform the user that their avatar was moved to another location
                                await ctx.RespondAsync($"{ctx.User.Mention}'s avatar was moved to `{_location.name}`");
                                
                                //break since the loop no longer needs to continue
                                break;
                            }
                        }*/
                        
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
        
        //**ATTACK**
        //Command : rpgAttack
        //This command is subject to a lot of change
        //Used to attack entities in a location
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
                                    await ctx.RespondAsync($"{ctx.User.Mention} is attacking `{_entity.name}` in `{users[ctx.Member].location.name}`.");
                                    await ctx.TriggerTypingAsync();
                                    _entity.health -= users[ctx.Member].damage;
                                    if (_entity.health <= 0)
                                    {
                                        await ctx.RespondAsync($"{ctx.User.Mention} has killed `{_entity.name}` at `{users[ctx.Member].location.name}`.");
                                        _location.entities.Remove(_entity);
                                        break;
                                    }
                                    else
                                    {
                                        await ctx.RespondAsync($"{ctx.User.Mention} has attacked `{_entity.name}` for `{users[ctx.Member].damage}` damage at `{users[ctx.Member].location.name}`.");
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
