using System;
using System.Collections.Generic;
using System.Text;

namespace ZuccBot.ZuccRPG.RPGassets
{
    public enum Die
    {
        d4,
        d6,
        d8,
        d10,
        d12,
        d20
    }

    public enum ArmorType
    {
        head,
        torso,
        leg,
        foot
    }

    public enum EquipmentSkill
    {
        simple,
        dextrious,
        strength,
        constitution
    }

    public class Item
    {
        public string name;//Used when the item is referenced in chat

        public Item(string _name)
        {
            name = _name;
        }
    }

    public class Weapon : Item
    {
        public Die die;
        public EquipmentSkill skill;

        public Weapon(Die _die, EquipmentSkill _skill, string _name) : base(_name)
        {
            die = _die;
            skill = _skill;
        }
    }

    public class Armor : Item
    {
        public Die defense;
        public ArmorType armor;
        public EquipmentSkill skill;

        public Armor(Die _defense, ArmorType _armor, EquipmentSkill _skill, string _name) : base(_name)
        {
            defense = _defense;
            armor = _armor;
            skill = _skill;
        }
    }
}
