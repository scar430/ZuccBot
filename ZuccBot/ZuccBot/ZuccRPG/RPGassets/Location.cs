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
    public class Location
    {
        public string name;
        public List<Entity> entities = new List<Entity>();
        public List<Item> items = new List<Item>();

        public Location(string _name, List<Entity> _entities, List<Item> _items)
        {
            name = _name;
            entities = _entities;
            items = _items;
        }
    }

    public class Room
    {

    }
}
