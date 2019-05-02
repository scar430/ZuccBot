using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ZuccBot.ZuccRPG.RPGassets
{
    public enum Population
    {
        Passive,
        Mixed,
        Aggressive
    }

    public class Location
    {
        public string name { get; set; }//What the location is referenced by.
        public Population population { get; set; }
        public List<Location> levels { get; set; }//All the levels, these are done in succession of each other (hence it being an IEnumerable).
        public List<Entity> entities = new List<Entity>();//All the entities in the location.
        public List<Item> items = new List<Item>();//All the entities in the location.

        public Location(string _name, Population _population, List<Location> _levels, List<Entity> _entities, List<Item> _items)
        {
            name = _name;
            population = _population;
            levels = _levels;
            entities = _entities;
            items = _items;
        }

        //Dungeons typically contain two enemies of the appropriate level and one of the next level (e.g. A Tier 1 Dungeon contains 2 Goblins and 1 Orc)
        //Theres 3 tiers (for this version)
        public void SpawnEnemies(DiscordGuild guild, int tier)
        {
            switch (tier)
            {
                case 1:
                    using (StreamReader locationStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{guild.Id}\\Locations.txt"))
                    using (JsonTextReader locationReader = new JsonTextReader(locationStreamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                        //Look for the 2 Goblins and 1 Orc that should be in this dungeon, if they aren't there then everything has been killed so repopulate the dungeon.
                        var dungeon = locations.Find(x => x.name == "Dungeon");

                        using (StreamReader entityStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\EntityConfig.txt"))
                        using (JsonTextReader entityReader = new JsonTextReader(entityStreamReader))
                        {
                            JsonSerializer _serializer = new JsonSerializer();
                            _serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            _serializer.NullValueHandling = NullValueHandling.Ignore;

                            List<Entity> entites = (List<Entity>)_serializer.Deserialize(entityReader, typeof(List<Entity>));

                            if (dungeon.entities.FindAll(x => x.name == "Goblin").Count == 0 && !dungeon.entities.Exists(x => x.name == "Orc"))
                            {
                                dungeon.entities.Add(entities.Find(x => x.name == "Goblin"));
                                dungeon.entities.Add(entities.Find(x => x.name == "Goblin"));
                                dungeon.entities.Add(entities.Find(x => x.name == "Orc"));
                            }
                        }
                    } 
                    break;
                case 2:
                    using (StreamReader locationStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{guild.Id}\\Locations.txt"))
                    using (JsonTextReader locationReader = new JsonTextReader(locationStreamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                        //Look for the 2 Goblins and 1 Orc that should be in this dungeon, if they aren't there then everything has been killed so repopulate the dungeon.
                        var dungeon = locations.Find(x => x.name == "Dungeon");

                        using (StreamReader entityStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\EntityConfig.txt"))
                        using (JsonTextReader entityReader = new JsonTextReader(entityStreamReader))
                        {
                            JsonSerializer _serializer = new JsonSerializer();
                            _serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            _serializer.NullValueHandling = NullValueHandling.Ignore;

                            List<Entity> entites = (List<Entity>)_serializer.Deserialize(entityReader, typeof(List<Entity>));

                            if (dungeon.entities.FindAll(x => x.name == "Orc").Count == 0 && !dungeon.entities.Exists(x => x.name == "Troll"))
                            {
                                dungeon.entities.Add(entities.Find(x => x.name == "Orc"));
                                dungeon.entities.Add(entities.Find(x => x.name == "Orc"));
                                dungeon.entities.Add(entities.Find(x => x.name == "Troll"));
                            }
                        }
                    }
                    break;
                case 3:
                    using (StreamReader locationStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\{guild.Id}\\Locations.txt"))
                    using (JsonTextReader locationReader = new JsonTextReader(locationStreamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                        //Look for the 2 Goblins and 1 Orc that should be in this dungeon, if they aren't there then everything has been killed so repopulate the dungeon.
                        var dungeon = locations.Find(x => x.name == "Dungeon");

                        using (StreamReader entityStreamReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\EntityConfig.txt"))
                        using (JsonTextReader entityReader = new JsonTextReader(entityStreamReader))
                        {
                            JsonSerializer _serializer = new JsonSerializer();
                            _serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            _serializer.NullValueHandling = NullValueHandling.Ignore;

                            List<Entity> entites = (List<Entity>)_serializer.Deserialize(entityReader, typeof(List<Entity>));

                            if (dungeon.entities.FindAll(x => x.name == "Troll").Count == 0)
                            {
                                dungeon.entities.Add(entities.Find(x => x.name == "Troll"));
                                dungeon.entities.Add(entities.Find(x => x.name == "Troll"));
                                dungeon.entities.Add(entities.Find(x => x.name == "Troll"));
                            }
                        }
                    }
                    break;
            }
        }
    }
}
