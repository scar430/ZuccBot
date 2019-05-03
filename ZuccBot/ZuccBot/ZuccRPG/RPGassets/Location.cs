using System;
using System.Collections.Generic;
using System.IO;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public int tier = 0;
        public Population population { get; set; }
        public List<Location> levels { get; set; }//All the levels, these are done in succession of each other (hence it being an IEnumerable).
        public List<Entity> entities = new List<Entity>();//All the entities in the location.
        public List<Item> items = new List<Item>();//All the entities in the location.

        public Location(string _name, int _tier, Population _population, List<Location> _levels, List<Entity> _entities, List<Item> _items)
        {
            name = _name;
            tier = _tier;
            population = _population;
            levels = _levels;
            entities = _entities;
            items = _items;
        }

        //This was done the quick and dirty way, be warned.
        //Dungeons typically contain two enemies of the appropriate level and one of the next level (e.g. A Tier 1 Dungeon contains 2 Goblins and 1 Orc)
        //Theres 3 tiers (for this version)
        public void SpawnEnemies(DiscordGuild guild)
        {
            switch (tier)
            {
                case 1:
                    using (StreamReader characterReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\EntityConfig.txt"))
                    using (JsonTextReader charJReader = new JsonTextReader(characterReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        List<CombatEntity> _entities = (List<CombatEntity>)serializer.Deserialize(charJReader, typeof(List<CombatEntity>));

                        //Look for the 2 Goblins and 1 Orc that should be in this dungeon, if they aren't there then everything has been killed so repopulate the dungeon
                        if (this.entities.FindAll(x => x.name == "Goblin").Count == 0 && !this.entities.Exists(x => x.name == "Orc"))
                        {
                            this.entities.Add(_entities.Find(x => x.name == "Goblin"));
                            this.entities.Add(_entities.Find(x => x.name == "Goblin"));
                            this.entities.Add(_entities.Find(x => x.name == "Orc"));

                            using (StreamReader itemSR = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\ItemConfig.txt"))
                            using (JsonTextReader itemJTR = new JsonTextReader(itemSR))
                            {
                                JsonSerializer _serializer = new JsonSerializer();
                                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                serializer.NullValueHandling = NullValueHandling.Ignore;

                                Dictionary<string, List<Item>> _items = (Dictionary<string, List<Item>>)_serializer.Deserialize(itemJTR, typeof(Dictionary<string, List<Item>>));

                                foreach (Entity entity in _entities)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var soICanReadThis = _items["Crude"][new Random().Next(0, _items["Crude"].Count - 1)];
                                        entity.items.Add(soICanReadThis);
                                        Console.WriteLine(soICanReadThis);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    //Look for the 2 Orcs and 1 Troll that should be in this dungeon, if they aren't there then everything has been killed so repopulate the dungeon.
                    if (this.entities.FindAll(x => x.name == "Orc").Count == 0 && !this.entities.Exists(x => x.name == "Troll"))
                    {
                        using (StreamReader characterReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\EntityConfig.txt"))
                        using (JsonTextReader charJReader = new JsonTextReader(characterReader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            serializer.NullValueHandling = NullValueHandling.Ignore;

                            List<CombatEntity> _entities = (List<CombatEntity>)serializer.Deserialize(charJReader, typeof(List<CombatEntity>));
                            this.entities.Add(_entities.Find(x => x.name == "Orc"));
                            this.entities.Add(_entities.Find(x => x.name == "Orc"));
                            this.entities.Add(_entities.Find(x => x.name == "Troll"));
                        }
                    }
                    break;
                case 3:
                    using (StreamReader characterReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\GenericRPG\\EntityConfig.txt"))
                    using (JsonTextReader charJReader = new JsonTextReader(characterReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        List<CombatEntity> _entities = (List<CombatEntity>)serializer.Deserialize(charJReader, typeof(List<CombatEntity>));
                        //Look for the 2 Goblins and 1 Orc that should be in this dungeon, if they aren't there then everything has been killed so repopulate the dungeon.
                        if (this.entities.FindAll(x => x.name == "Troll").Count == 0)
                        {
                            this.entities.Add(_entities.Find(x => x.name == "Troll"));
                            this.entities.Add(_entities.Find(x => x.name == "Troll"));
                            this.entities.Add(_entities.Find(x => x.name == "Troll"));
                        }
                    }
                    break;
            }
        }
    }
}
