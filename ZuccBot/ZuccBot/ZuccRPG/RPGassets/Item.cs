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
            Random random = new Random();
            int roll = 0;

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

            if (roll == 1)
            {
                return $" has missed their target.";
            }
            else
            {
                switch (skill)
                {
                    default:
                        defender.curHP -= roll;
                        if (defender.curHP > 0)
                        {
                            return $"{attacker.name} has dealt {roll} damage to {defender.name}.";
                        }
                        else
                        {
                            using (StreamReader locationReader = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPG\\" + guild.Id + "\\Locations.txt"))
                            {
                                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                                List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                                location.entities.Remove(defender as Entity);
                            }
                            return $"{attacker.name} has slain {defender.name}.";
                        }
                    case EquipmentSkill.strength:
                        defender.curHP -= roll + attacker.strength;
                        if (defender.curHP > 0)
                        {
                            return $"{attacker.name} has dealt {roll + attacker.strength} damage to {defender.name}.";
                        }
                        else
                        {
                            using (StreamReader locationReader = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPG\\" + guild.Id + "\\Locations.txt"))
                            {
                                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                                List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                                location.entities.Remove(defender as Entity);
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
                            using (StreamReader locationReader = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPG\\" + guild.Id + "\\Locations.txt"))
                            {
                                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                                List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                                location.entities.Remove(defender as Entity);
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
                            List<Location> newLocations = new List<Location>();
                            using (StreamReader locationReader = File.OpenText(Directory.GetCurrentDirectory() + "\\GenericRPG\\" + guild.Id + "\\Locations.txt"))
                            {
                                ITraceWriter tcr = new MemoryTraceWriter();//This is more of a debug thing, it's just checking in on you and what your doing.

                                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { TraceWriter = tcr });

                                List<Location> locations = (List<Location>)serializer.Deserialize(locationReader, typeof(List<Location>));

                                location.entities.Remove(defender as Entity);
                            }
                            return $"{attacker.name} has slain {defender.name}.";
                        }
                }
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
