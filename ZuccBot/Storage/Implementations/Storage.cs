using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Core
{
    class Storage : IDataStorage
    {
        private readonly Dictionary<string, object> _dict = new Dictionary<string, object>();//This is readonly since you don't need to be messing around with this, more of a "just in case this comes in handy" type of thing.

        public T RestoreObject<T>(string key)
        {
            Console.WriteLine("DEBUG : Started DiscordBot.Core.RestoreObject(string key)");
            if (!_dict.ContainsKey(key))
            {
                Console.WriteLine("DEBUG : DiscordBot.Core.Storage._dict did not contain the desired key.");
                throw new ArgumentException($"Key '{key}' is absent. Your providing the cash but we ain't sellin'. ");
            }

            Console.WriteLine("DEBUG : Ended DiscordBot.Core.Storage.RestoreObject<T>(string key)");
            return (T)_dict[key];//Casting because (why not?) the object in question doesn't really 'exist' yet.
        }

        public void StoreObject(object obj, string key)
        {
            Console.WriteLine("DEBUG : Started DiscordBot.Core.Storage.StoreObject(object obj, string key)");
            if (_dict.ContainsKey(key))
            {
                Console.WriteLine("DEBUG : DiscordBot.Core.Storage._dict already contains the desired key.");
                return;
            }
            else
            {
                _dict.Add(key, obj);
                Console.WriteLine("DEBUG : DiscordBot.Core.Storgage._dict did not contain the desired key and has added it to the DiscordBot.Core.Storage._dict");
            }
            Console.WriteLine("DEBUG : Ended DiscordBot.Core.Storage.StoreObject(object obj, string key)");
        }
    }
}
