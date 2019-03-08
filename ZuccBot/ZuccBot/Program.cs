using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace ZuccBot
{
    class Program
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NTUzNDYwMTEwMzI5MTE4NzIx.D2Rqeg.6ATVmDT5kF4XF_3YUTjMxZj8SDE",
                TokenType = TokenType.Bot
            });

            //Not the original Pong but it's definitely something.
            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                {
                    Console.WriteLine("pong!");
                    await e.Message.RespondAsync("pong!");
                }
            };

            await discord.ConnectAsync();//Is anyone listening, am I all alone?
            await Task.Delay(-1);//Wait infinitely. Bot purgatory.
        }
    }
}
