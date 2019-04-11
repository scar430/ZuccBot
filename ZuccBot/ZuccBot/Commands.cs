using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ZuccBot
{
    public class Commands
    {
        
        //Shut Down command
        [Command("ShutDown"), Aliases("off", "shutdown", "shutoff", "die", "death", "sleep"), RequireOwner, Hidden]
        public async Task ShutOff(CommandContext ctx)
        {
            await ctx.RespondAsync($"ZuccBot is turning off.");
            Environment.Exit(420);//huehuehuehue
        }
        
        [Command("log"), Description("Records messages by Guild, Channel, and User")]
        public async Task Log(CommandContext ctx)
        {
            //try
            //{
            /*foreach (DiscordGuild guild in Program.discord.Guilds.Values)
            {
                await Program.discord.SendMessageAsync(guild.GetDefaultChannel(), $"{Program.discord.CurrentUser.Mention} is now ***RECORDING*** everything that is being said.");
            }*/
            var listener = Program.discord.GetInteractivityModule();
            MessageContext msg = await listener.WaitForMessageAsync(null, TimeSpan.FromMinutes(-1));

            //This could be done with an SQL DB however this was on hand and I doubt you will get enough messages for this to cause problems, however if you are getting mass amounts of messages a JSON (txt in this case, the main point is your writing to physical memory) then the messages will start to be discarded if they're coming in volumes that the HDD can't handle. I don't know if this is the same in the case of a SSD but I'm assuming your using a HDD because SSDs are for filthy casuals.
            using (StreamWriter _file = File.CreateText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + msg.Guild.Name.ToString() + "\\" + msg.Channel.Name.ToString() + "\\" + msg.Channel.Name.ToString() + ".txt"))
            {
                using (StreamWriter file = File.AppendText(Directory.GetCurrentDirectory() + "\\ChatLogs\\" + msg.Guild.Name.ToString() + "\\" + msg.Channel.Name.ToString() + "\\" + msg.Channel.Name.ToString() + ".txt"))
                {
                    file.WriteLine("Date : " + DateTime.Today.ToString() + " | " + "Time : " + DateTime.Today.TimeOfDay.ToString() + " | " + "Guild : " + msg.Guild.ToString() + " | " + "Channel : " + msg.Channel.Name.ToString() + " | " + "User : " + msg.User.Username.ToString());
                }
            }

            await Task.Delay(-1);
            //}
            /*catch
            {
                try
                {
                    foreach (DiscordGuild guild in Program.discord.Guilds.Values)
                    {
                        await Program.discord.SendMessageAsync(guild.GetDefaultChannel(), $"{Program.discord.CurrentUser.Mention} is ***NOT RECORDING*** everything that is being said.");
                    }
                    await ctx.Channel.SendMessageAsync($"The chat log has failed.");
                    Console.WriteLine(Console.Error.ToString());
                    await Task.CompletedTask;
                }
                catch
                {
                    Console.WriteLine("The chat log has failed.");
                    Console.WriteLine(Console.Error.ToString());
                    await Task.CompletedTask;
                }
            }*/
        }        

        //Basic hi commmand
        [Command("hi"), Description("Say hello!")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
        }

        /* Basic ping command.
         * Command : ping
         */
        [Command("ping"), Description("Replys with 'pong!'")]
        public async Task Pong(CommandContext ctx)
        {
            await ctx.RespondAsync($"pong!, {ctx.User.Mention}!");
        }

        /* Post School website
         * Command : columbia
         */
        [Command("columbia"), Aliases("school", "college"), Description("Posts the schools website.")]
        public async Task columbia(CommandContext ctx)
        {
            //DiscordWebhook webhook = new DiscordWebhook();
            await ctx.RespondAsync($"Hey, {ctx.User.Mention}, here's a link to Columbia's website: https://www.gocolumbia.edu/");
        }

        //Was gonna implement this at the request of Chris Gregory, however I abondoned it because I found something more interesting to do.
        [Command("bugHunt")]
        public async Task bugHunt(CommandContext ctx)
        {
            await ctx.RespondWithFileAsync("C:/Users/scar4/Desktop/Data/GitHubRepositories/ZuccBot/Assets");
            Console.WriteLine("Uploading Bug Hunt picture");
        }

        /*What time did the mentioned user join
         * Command : userJoined @User
         */
        [Command("userJoined"), Aliases("joined", "join", "userJoin", "userjoin", "userjoined"), Description("What time did the mentioned user join the server?")]
        public async Task joined(CommandContext ctx, DiscordMember member)
        {

            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();

            string date = member.JoinedAt.ToString();//Create a string because some weird things were happening when it wasn't.

            //Prioritizes nickname, if you don't have a nickname it shows your display name, I want it to avoid mentioning people cuz that is annoying.
            if (member.Nickname != null)
            {
                await ctx.RespondAsync($"{member.Nickname} joined on {date}");
            }
            else
            {
                await ctx.RespondAsync($"{member.DisplayName} joined on {date}");
            }
        }

        /*Thank the Bot
         * Command : thanks
         */
        [Command("thanks"), Aliases("thankyou", "Thanks", "Thankyou"), Description("Be polite, say 'thank you' !")]
        public async Task thanks(CommandContext ctx)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"Your welcome!");
        }

        /* Grant a Role to a user.
         * Command : addRole @User @Role
         */
        [Command("addRole"), Aliases("role", "aRole", "addrole", "arole"), RequirePermissions(Permissions.Administrator), Description("Add a role to a player."), Hidden]
        public async Task addRole(CommandContext ctx, DiscordMember member, DiscordRole role)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await ctx.Guild.GrantRoleAsync(member, role);
            await ctx.RespondAsync($"Granted {role} to {member.DisplayName}.");
        }

        /* Revoke a Role from a user.
         * Command : removeRole @User @Role [reason for revoking role]
         */
        [Command("removeRole"), Aliases("rRole", "removerole", "rrole"), RequirePermissions(Permissions.Administrator), Description("Remove a users role, it MUST include a reason at the end (you can't pass in null)."), Hidden]
        public async Task removeRole(CommandContext ctx, DiscordMember member, DiscordRole role, string reason)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await ctx.Guild.RevokeRoleAsync(member, role, reason);
            await ctx.RespondAsync($"Revoked {member.DisplayName}'s role as {role} because of ''{reason}''.");
        }


        /* Kick a User
         * Command : kick @User
         */
        [Command("kickUser"), Aliases("kick", "Kick"), RequirePermissions(Permissions.Administrator), Description("Kick mentioned users from the server. This does not work on users with the 'Administrator' Permission"), Hidden]
        public async Task kickUser(CommandContext ctx, DiscordMember member)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await ctx.Guild.RemoveMemberAsync(member);//Removing the mentioned user from the Guild.
            await ctx.RespondAsync($"Kicked {member.DisplayName} from {ctx.Guild}.");//Just the Bot telling everyone what it is doing
        }

        [Command("banUser"), Aliases("ban", "Ban"), RequirePermissions(Permissions.Administrator), Description("Used to BAN mentioned users for the set amount of days"), Hidden]
        public async Task banUser(CommandContext ctx, DiscordMember member, int days, string reason)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await ctx.Guild.BanMemberAsync(member, days, reason);
            await ctx.RespondAsync($"Banned {member} from {ctx.Guild} for ''{reason}''. The Ban will be lifted {days} days from now.");
        }

        [Command("retrieveBans"), Aliases("findBans", "bans", "Bans", "FindBans"), Description("Lists all bans.")]
        public async Task retrieveBans(CommandContext ctx)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"List of Bans from {ctx.Guild} : ");//Just the bot telling everyone what it is posting.
            await ctx.Guild.GetBansAsync();
        }

        //Due to a problem with connecting a mentioned user to a channel and muting them from that channel, these mute commands do not work
        //Broken
        [Command("muteUser"), Aliases("mute", "muteuser"), Description("Mute the mentioned user, this is for NO set time and you MUST use unmute to unmute the muted player"), RequirePermissions(Permissions.Administrator), Hidden]
        public async Task muteUser(CommandContext ctx, DiscordMember member, string reason)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await member.SetMuteAsync(true, reason);//This is incorrect because I only mentioned the user and not in what channel they are being mute.
            await ctx.RespondAsync($"Muted {member.DisplayName} for '{reason}'");//Just the bot telling everyone what it's doing.
        }

        //Broken
        [Command("unmuteUser"), Aliases("unmute", "unmuteuser", "nomute", "removemute", "rmute", "nmute", "omittmute", "omute"), RequirePermissions(Permissions.Administrator), Description("Unmute the mentioned user."), Hidden]
        public async Task unmuteUser(CommandContext ctx, DiscordMember member, string reason)
        {
            // typing indicator to make Mr. Zuckerburg feel more human.
            //await ctx.TriggerTypingAsync();
            await member.SetMuteAsync(false, reason);//Not sure how to reference the channel and the player and unmute them in that.
            await ctx.RespondAsync($"Unmuted {member.DisplayName}.");//Just the bot telling everyone what it's doing.
        }
    }
}
