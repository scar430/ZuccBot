using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.DiscordBots.ZuccBot.Games.GenericRPG
{
    public class Entity
    {
        public string name;//Name of the entity, often used in messages
        public int health;//Health of the entity
        public List<Item> items = new List<Item>();//Inventory of the entity
        public Location location;//Current location of the entity

        //Gold, Moola, etc.
        int money;

        //Armor slots that can contian Items.
        public Item headSlot;//Equip Items to the head.
        public Item torsoSlot;//Equip Items to the Torso.
        public Item legSlot;//Equip Items to the Legs.
        public Item footSlot;//Equip Items to the Feet.

        //Tool slots that can contain Items.
        public Item primary;//Primary hand, like a sword.
        public Item secondary;//Secondary hand, like a shield, torch, or potion.

        public Entity(string _name, int _damage, int _health, Location _location, int money)
        {

        }
    }
}
