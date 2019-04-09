using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;

namespace Main.ZuccRPG.RPGassets
{
    //Necessities for every entity type
    public class Entity
    {
        public string name { get; set; }//Name of the entity, often used in messages
        public List<Item> items = new List<Item>();//Inventory of the entity

        enum Race
        {
            Human,
            Elf,
            Dwarf,
            Goblin
        }

        enum Class
        {
            Knight,
            Ranger,
            Rouge,
            Wizard
        }

        public int ac { get; set; }//Armor Class (d&d)
        public int maxiumHP { get; set; }//Maximum Hit Points. This is used to stop the player and other entities from using healing items constantly to MASSIVELY buff their health
        public int curHP { get; set; }//What the current Hit Point score is at, This can not exceed the maxiumHP integer and if it reaches 0 or lower then death will occur

        //Armor slots that can contian Items.
        public Item headSlot { get; set; }//Equip Items to the head.
        public Item torsoSlot { get; set; }//Equip Items to the Torso.
        public Item legSlot { get; set; }//Equip Items to the Legs.
        public Item footSlot { get; set; }//Equip Items to the Feet.

        public int strength { get; set; }
        public int dexterity { get; set; }
        public int constitution { get; set; }
        public int intelligence { get; set; }
        public int widsom { get; set; }
        public int charisma { get; set; }

        public Entity(string _name, List<Item> _items)
        {
            name = _name;
            items = _items;
        }
    }
}
