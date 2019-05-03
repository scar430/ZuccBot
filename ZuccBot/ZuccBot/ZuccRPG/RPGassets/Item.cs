using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Net;
using DSharpPlus.Entities;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZuccBot.ZuccRPG.RPGassets
{
    public enum Die
    {
        d4 = 0,
        d6 = 1,
        d8 = 2,
        d10 = 3,
        d12 = 4,
        d20 = 5
    }

    public enum ArmorType
    {
        head = 0,
        torso = 1,
        leg = 2,
        foot = 3
    }

    public enum EquipmentSkill
    {
        simple = 0,//This doesn't add any skill modifiers to damage or defense, typically simple weapons are gonna be doing less damage.
        dextrious = 1,
        strength = 2,
        constitution = 3
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

        //Combat favors the attacker.
        //If the character rolls a one they miss.
        public string Attack(CombatEntity attacker, CombatEntity defender, Location location, DiscordGuild guild)
        {
            if (attacker.curHP > 0)
            {
                Random random = new Random();
                int roll = 0;

                //Roll depending on what attack die you have
                switch (die)
                {
                    case Die.d4:
                        roll = random.Next(0, 5);
                        break;
                    case Die.d6:
                        roll = random.Next(0, 7);
                        break;
                    case Die.d8:
                        roll = random.Next(0, 9);
                        break;
                    case Die.d10:
                        roll = random.Next(0, 11);
                        break;
                    case Die.d12:
                        roll = random.Next(0, 13);
                        break;
                    case Die.d20:
                        roll = random.Next(0, 21);
                        break;
                }

                //This is a miss
                if (roll == 1)
                {
                    return $"{attacker.name} has missed their target.";
                }
                else
                {
                    //The weapon is looking to match its modifier with the players skills (If you have a strength based weapon you want high strength stats)
                    switch (skill)
                    {
                        default:
                            //Minus health
                            defender.curHP -= roll;
                            //Is defender dead?
                            if (defender.curHP > 0)
                            {
                                //If not dead, return how much damage we did
                                return $"{attacker.name} has dealt {roll} damage to {defender.name}.";
                            }
                            else
                            {
                                //Remove the entity from the game
                                location.entities.Remove(defender as Entity);

                                //Hand out loot
                                foreach (Item item in defender.items)
                                {
                                    attacker.items.Add(item);
                                }

                                //Return death message
                                return $"{attacker.name} has slain {defender.name}.";

                                //It's the same for the rest of them
                            }
                        case EquipmentSkill.strength:
                            defender.curHP -= roll + attacker.strength;
                            if (defender.curHP > 0)
                            {
                                return $"{attacker.name} has dealt {roll + attacker.strength} damage to {defender.name}.";
                            }
                            else
                            {
                                location.entities.Remove(defender as Entity);
                                foreach (Item item in defender.items)
                                {
                                    attacker.items.Add(item);
                                }
                                return $"{attacker.name} has slain {defender.name}.";
                            }
                        case EquipmentSkill.dextrious:
                            defender.curHP -= roll + attacker.dexterity;
                            if (defender.curHP > 0)
                            {
                                return $"{attacker.name} has dealt {roll + attacker.dexterity} damage to {defender.name}.";
                            }
                            else
                            {
                                location.entities.Remove(defender as Entity);
                                foreach (Item item in defender.items)
                                {
                                    attacker.items.Add(item);
                                }
                                return $"{attacker.name} has slain {defender.name}.";
                            }
                        case EquipmentSkill.constitution:
                            defender.curHP -= roll + attacker.constitution;
                            if (defender.curHP > 0)
                            {
                                return $"{attacker.name} has dealt {roll + attacker.constitution} damage to {defender.name}.";
                            }
                            else
                            {
                                location.entities.Remove(defender as Entity);
                                foreach (Item item in defender.items)
                                {
                                    attacker.items.Add(item);
                                }
                                return $"{attacker.name} has slain {defender.name}.";
                            }
                    }
                }
            }
            else
            {
                //We aren't alive so return failed attack message
                return $"{attacker.name}'s lifeless body twitches.";
            }
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
