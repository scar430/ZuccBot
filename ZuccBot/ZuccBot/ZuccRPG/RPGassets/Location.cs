using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

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
        public IEnumerable<Location> levels { get; set; }//All the levels, these are done in succession of each other (hence it being an IEnumerable).
        public Dictionary<string[], Entity> entities = new Dictionary<string[], Entity>();//All the entities in the location.
        public List<Item> items = new List<Item>();//All the entities in the location.

        public Location(string _name, Population _population, IEnumerable<Location> _levels, Dictionary<string[], Entity> _entities, List<Item> _items)
        {
            name = _name;
            population = _population;
            levels = _levels;
            entities = _entities;
            items = _items;
        }

        public void arrangeEntities()
        {
            Dictionary<string, List<Entity>> namesToSort = new Dictionary<string, List<Entity>>();
            foreach(Entity value in entities.Values)
            {
                if (namesToSort.ContainsKey(value.name))
                {
                    namesToSort[value.name].Add(value);
                }
                else
                {
                    List<Entity> newList;
                    namesToSort.Add(value.name, newList = new List<Entity>() { value });
                }
            }

            entities.Clear();

            foreach (string key in namesToSort.Keys)
            {
                for (int i = 0; i < namesToSort[key].Count - 1; i++)
                {
                    string[] newString;
                    entities.Add(newString = new string[] { key, i.ToString() }, namesToSort[key][i]);
                }
            }
        }
    }
}
