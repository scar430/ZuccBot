using System.Collections.Generic;
using System;

namespace ZuccBot.ZuccRPG.RPGassets
{
    public enum Race
    {
        Human,
        Elf,
        Dwarf,
        Goblin,
        Orc,
        Troll,
    }

    public enum Class
    {
        Knight,
        Rouge,
        Mage
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
        public Race race;//Species, (e.g. Elf) adds a stat modifier
        public Class @class;//Profession, (e.g. Knight) adds a stat modifier

        public int maxHP = 0;//Maximum Hit Points. This is used to stop the player and other entities from using healing items constantly to MASSIVELY buff their health
        public int curHP = 0;//What the current Hit Point score is at, This can not exceed the maxiumHP integer and if it reaches 0 or lower then death will occur

        //Armor slots that can contian Items.
        public Item headSlot { get; set; }//Equip Items to the head.
        public Item torsoSlot { get; set; }//Equip Items to the Torso.
        public Item legSlot { get; set; }//Equip Items to the Legs.
        public Item footSlot { get; set; }//Equip Items to the Feet.

        public int strength = 0;//Modifier with strength based weapons.
        public int dexterity = 0;//Modifier with dexterity based weapons.
        public int constitution = 0;//Modifier with constitution based weapons.

        //Constructor
        public CombatEntity(Race _race, Class _class, int _maxHP, int _strength, int _dexterity, int _constitution, string _name, List<Item> _items) : base(_name, _items)
        {
            race = _race;
            @class = _class;
            maxHP = _maxHP;
            maxHP = curHP;
            Console.WriteLine(curHP);
            strength = _strength;
            dexterity = _dexterity;
            constitution = _constitution;
        }
    }
}