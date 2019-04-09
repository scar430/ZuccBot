using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;

namespace Main.ZuccRPG.RPGassets
{
    public enum Race
    {
        Human,
        Elf,
        Dwarf,
        Goblin
    }

    public enum Class
    {
        Knight,
        Ranger,
        Rouge,
        Wizard
    }
    public class Entity
    {
        public string name { get; set; }//Name of the entity, often used in messages
        public List<Item> items = new List<Item>();//Inventory of the entity

        public Entity(string _name, List<Item> _items)
        {
            name = _name;
            items = _items;
        }
    }

    public class CombatEntity : Entity
    {
        Race race { get; set; }
        Class @class { get; set; }

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

        public CombatEntity(Race _race, Class _class, int _strength, int _dexterity, int _constitution, int _intelligence, int _widsom, int _charisma, string _name, List<Item> _items) : base(_name, _items)
        {
            race = _race;
            @class = _class;
            strength = _strength;
            dexterity = _dexterity;
            constitution = _constitution;
            intelligence = _intelligence;
            widsom = _widsom;
            charisma = _charisma;
        }
    }

    public class Enemy : CombatEntity
    {
        public Enemy(Race _race, Class _class, int _strength, int _dexterity, int _constitution, int _intelligence, int _widsom, int _charisma, string _name, List<Item> _items) : base(_race, _class, _strength, _dexterity, _constitution, _intelligence, _widsom, _charisma, _name, _items)
        {

        }
    }
}
