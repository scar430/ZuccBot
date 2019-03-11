using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Discord.DiscordBots.ZuccBot.Games.GenericRPG
{
    public class GenericRPG
    {
        //***NOTE*** Games are server based so your characters can not interact with games on other servers and can not be carried over to other servers.
        const string commandPrefix = "rpg";
        List<Location> locations = new List<Location>();//All the locations in this world.

        Dictionary<DiscordMember, Entity> users = new Dictionary<DiscordMember, Entity>();//Dictionary<Key, Value> (It's basically a HashMap from Java) that is used to pair up Discord Users with their character

        [Command(commandPrefix + "CreateWorld"), Aliases(commandPrefix + "StartWorld", commandPrefix + "startworld", commandPrefix + "start"), RequirePermissions(Permissions.Administrator), Hidden]
        public async Task CreateWorld(CommandContext ctx)
        {
            await ctx.RespondAsync($"Started to create a new world...");
            await ctx.TriggerTypingAsync();
            Location tavern = new Location("Tavern", null, null);
            locations.Add(tavern);

            Location mine = new Location("Mine", null, null);
            locations.Add(mine);

            Location pawnShop = new Location("PawnShop", null, null);
            locations.Add(pawnShop);


            Entity goblin = new Entity("Goblin", 1, 5, locations[locations.Count - 1]);
            List<Entity> dungeonEntities = new List<Entity>();
            dungeonEntities.Add(goblin);
            Location dungeon = new Location("Dungeon", dungeonEntities, null);
            locations.Add(dungeon);
            await ctx.RespondAsync($"Created a new world to explore!");
        }

        [Command(commandPrefix + "GoTo"), Aliases(commandPrefix + "goto", commandPrefix + "go", commandPrefix + "to")]
        public async Task GoTo(CommandContext ctx, string location)
        {
            try
            {
                foreach (Location _location in locations)
                {
                    if (location == _location.name)
                    {
                        foreach (DiscordMember _member in users.Keys)
                        {
                            if(_member == ctx.Member)
                            {
                                users[_member].location = _location;
                                await ctx.RespondAsync($"{ctx.User.Mention}'s avatar was moved to `{_location.name}`");
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                //If an exception was thrown it asks if the player created a character
                await ctx.RespondAsync($"A serious problem has occurred.");
            }
            
        }

        [Command(commandPrefix + "LocationInformation"), Aliases(commandPrefix + "locInfo", commandPrefix + "hereinfo", commandPrefix + "infohere", commandPrefix + "infoHere", commandPrefix + "LocInfo")]
        public async Task LocationInformation(CommandContext ctx)
        {
            try
            {
                foreach (DiscordMember _member in users.Keys)
                {
                    if (_member == ctx.Member)
                    {
                        await ctx.RespondAsync($"Information on `{users[_member].location.name}`: \n**Name** : \n`{users[_member].location.name}`\n");
                        if(users[_member].location.entities != null && users[_member].location.entities.Count > 0)
                        {
                            await ctx.RespondAsync($"**Entities** : \n");
                            foreach(Location _location in locations)
                            {
                                if (_location.name == users[_member].location.name)
                                {
                                    foreach (Entity _entity in users[_member].location.entities)
                                    {
                                        await ctx.RespondAsync($"`{_entity.name}`\n");
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch
            {
                //If an exception was thrown (probaby meaning something was null, usually something to do with the location or location.entities being null) it then asks them if they made one yet.
                await ctx.RespondAsync($"A serious problem has occurred.");
            }
        }

        [Command(commandPrefix + "WhatLocations")]
        public async Task WhatLocations(CommandContext ctx)
        {
            await ctx.RespondAsync($"Here's a list of locations : ");
            for (int i = 0; i < (locations.Count); i++)
            {
                await ctx.RespondAsync($"\n`{locations[i].name}`");
            }
        }

        [Command(commandPrefix + "CreateCharacter")]
        public async Task CreateCharacter(CommandContext ctx)
        {
            await ctx.RespondAsync($"Started creating a character for {ctx.User.Mention}...");
            await ctx.TriggerTypingAsync();
            Entity player = new Entity(ctx.Member.DisplayName, 2, 100, locations[0]);
            users.Add(ctx.Member, player);
            await ctx.RespondAsync($"Finished creating a character for {ctx.User.Mention}! \nHappy trails, partner!");
        }

        [Command(commandPrefix + "Players")]
        public async Task Players(CommandContext ctx)
        {
            foreach(DiscordMember _member in users.Keys)
            {
                await ctx.RespondAsync($"{_member.DisplayName}\n");
            }
        }

        [Command(commandPrefix + "Attack"), Aliases("attack", "fight", "hit", "Fight", "Hit")]
        public async Task Attack(CommandContext ctx, string name)
        {
            try
            {
                foreach (DiscordMember _member in users.Keys)
                {
                    if (_member == ctx.Member)
                    {
                        if (users[_member].location.entities != null && users[_member].location.entities.Count > 0)
                        {
                            foreach (Location _location in locations)
                            {
                                if (_location.name == users[_member].location.name)
                                {
                                    foreach (Entity _entity in users[_member].location.entities)
                                    {
                                        if (_entity.name == name)
                                        {
                                            await ctx.RespondAsync($"{ctx.User.Mention} is attacking `{_entity.name}` in `{users[_member].location.name}`.");
                                            await ctx.TriggerTypingAsync();
                                            _entity.health -= users[_member].damage;
                                            if (_entity.health <= 0)
                                            {
                                                await ctx.RespondAsync($"{ctx.User.Mention} has killed `{_entity.name}` at `{users[_member].location.name}`.");
                                                _location.entities.Remove(_entity);
                                                break;
                                            }
                                            else
                                            {
                                                await ctx.RespondAsync($"{ctx.User.Mention} has attacked `{_entity.name}` for `{users[_member].damage}` damage at `{users[_member].location.name}`.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            await ctx.RespondAsync($"There are no entities to attack in `{users[_member].location.name}`.");
                        }
                    }
                }
            }
            catch
            {
                await ctx.RespondAsync($"A serious problem has occurred.");
            }
        }
    }
}
