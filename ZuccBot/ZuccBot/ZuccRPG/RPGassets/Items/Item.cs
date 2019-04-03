using System;
using System.Collections.Generic;
using System.Text;

namespace ZuccBot.ZuccRPG.Generic
{
    public enum Die
    {
        none,//Basically null
        d4,
        d6,
        d8,
        d10,
        d12,
        d20
    }
    public class Item
    {
        string name;//Used when the item is referenced in chat

        //Parse in 0 if you don't want this to offer anything. (e.g. You don't want a potion to act as a sword, construct with 0)
        Die die;//How many hit points does this deal when used as a weapon?

        //Parse in 0 if you don't want this to offer anything. (e.g. You don't want a potion to act as a helmet, construct with 0)
        int ac;//How many hit points does this protect from when equipped as armor

        Item(string _name, Die _die, int _ac)
        {
            name = _name;
            die = _die;
            ac = _ac;
        }
    }
}
