using System;
using System.Collections.Generic;
using System.Text;

namespace ZuccBot.ZuccRPG
{
    class Race
    {
        string name { get; set; }// Name of the race
        int asi { get; set; }//'asi' stands for 'Ability Score Increase'
        Dictionary<string, int> subraces = new Dictionary<string, int>();//Name of the subrace and the ability score it offers
    }
}
