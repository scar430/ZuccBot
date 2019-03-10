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


            Entity goblin = new Entity("Goblin", 1, 5, locations[locations.Count - 1], 1);
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
                                await ctx.RespondAsync($"{ctx.Member.DisplayName}'s avatar was moved to `{_location.name}`");
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
                        await ctx.RespondAsync($"Information on '{users[_member].location.name}' : \n**Name** : `{users[_member].location.name}`\n");
                        await ctx.RespondAsync($"`**Entities** : \n");
                        for(int i = 0; i < (users[_member].location.entities.Count); i++)
                        {
                            await ctx.RespondAsync($"{users[_member].location.entities[i].name}");
                        }
                        await ctx.RespondAsync($"`");
                        break;
                    }
                }
            }
            catch
            {
                //If an exception was thrown (probaby meaning the user never made a character) it then asks them if they made one yet.
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
            await ctx.RespondAsync($"Started creating a character for {ctx.Member.DisplayName}...");
            await ctx.TriggerTypingAsync();
            Entity player = new Entity(ctx.Member.DisplayName, 2, 100, locations[0], 5);
            users.Add(ctx.Member, player);
            await ctx.RespondAsync($"Finished creating a character for {ctx.Member.DisplayName}! \nHappy trails, partner!");
        }

        [Command(commandPrefix + "Players")]
        public async Task Players(CommandContext ctx)
        {
            foreach(DiscordMember _member in users.Keys)
            {
                await ctx.RespondAsync($"{_member.DisplayName}\n");
            }
        }
    }
}
