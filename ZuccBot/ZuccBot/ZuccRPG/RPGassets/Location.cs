using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Main.ZuccRPG.RPGassets
{
    //this will determine difficulty and the types of entites/items you will encounter
    public enum Region
    {
        Plains,//easy
        Hills,//moderate
        Mines,//hard
        Mountains,//punshing
    }

    public enum Population
    {
        Passive,
        Mixed,
        Aggressive
    }

    public class Location
    {
        public string name;//What the location is referenced by.
        public Region region;//The basically determines the difficulty of the location
        public Population population { get; set; }
        public IEnumerable<Location> levels { get; set; }//All the levels, these are done in succession of each other (hence it being an IEnumerable).
        public List<Entity> entities = new List<Entity>();//All the entities in the location.
        public List<Item> items = new List<Item>();//All the entities in the location.

        public Location(string _name, Region _region, Population _population, IEnumerable<Location> _levels, List<Entity> _entities, List<Item> _items)
        {
            name = _name;
            region = _region;
            population = _population;
            levels = _levels;
            entities = _entities;
            items = _items;
        }
    }
}
