using System;
using System.Collections.Generic;
using System.Text;

namespace ZuccBot.ZuccRPG.Generic
{
    public class Entity
    {
        public string name;//Name of the entity, often used in messages
        public int health;//Health of the entity
        public List<Entity> entities = new List<Entity>();
        public List<Item> items = new List<Item>();//Inventory of the entity
        public Location location;//Current location of the entity

        //Armor slots that can contian Items.
        public Item headSlot;//Equip Items to the head.
        public Item torsoSlot;//Equip Items to the Torso.
        public Item legSlot;//Equip Items to the Legs.
        public Item footSlot;//Equip Items to the Feet.

        //Tool slots that can contain Items.
        public Item primary;//Primary hand, like a sword.
        public Item secondary;//Secondary hand, like a shield, torch, or potion.

        //Currently for debugging purposes
        public int damage;

        //these will be implemented later
        public int strength = 0;
        public int dexterity = 0;
        public int constitution = 0;
        public int intelligence = 0;
        public int widsom = 0;
        public int charisma = 0;

        public Entity(string _name, int _damage, int _health, Location _location)
        {
            name = _name;
            damage = _damage;
            health = _health;
            location = _location;
        }
    }
}
