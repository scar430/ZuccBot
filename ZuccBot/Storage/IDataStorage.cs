using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Core
{
    interface IDataStorage
    {
        void StoreObject(object obj, string key);//at least I didn't name them object and string ;)

        T RestoreObject<T>(string key);
    }
}
