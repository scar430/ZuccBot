using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ZuccBot.Commands
{
    public class ZuccCommands
    {
        /*
        //Shut Down command
        [Command("ShutDown"), Aliases("off", "shutdown", "shutoff", "die", "death", "sleep")]
        public async Task ShutOff(CommandContext ctx)
        {
            
        }
        */

        //Basic hi commmand
        [Command("hi"), Description("Say hello!")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
        }

        //Basic ping command.
        [Command("ping"), Description("Replys with 'pong!'")]
        public async Task Pong(CommandContext ctx)
        {
            await ctx.RespondAsync($"pong!, {ctx.User.Mention}!");
        }

        //Post School website
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

        //This just ask what time the mentioned user joined the server
        [Command("userJoined"), Aliases("joined", "join", "userJoin", "userjoin", "userjoined"), Description("What time did the mentioned user join the server?")]
        public async Task joined(CommandContext ctx, DiscordMember member)
        {
            // note the [RemainingText] attribute on the argument.
            // it will capture all the text passed to the command

            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // get the command service, we need this for
            // sudo purposes
            var cmds = ctx.CommandsNext;

            string date = member.JoinedAt.ToString();

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

        //Be polite!
        [Command("thanks"), Aliases("thankyou", "Thanks", "Thankyou"), Description("Be polite, say 'thank you' !")]
        public async Task thanks(CommandContext ctx)
        {
            await ctx.RespondAsync($"Your welcome!");
        }

        [Command("addRole"), Aliases("role", "aRole", "addrole", "arole"), RequirePermissions(Permissions.Administrator), Description("Add a role to a player."), Hidden]
        public async Task addRole(CommandContext ctx, DiscordMember member, DiscordRole role)
        {
            await ctx.Guild.GrantRoleAsync(member, role);
            await ctx.RespondAsync($"Granted {role} to {member.DisplayName}.");
        }

        [Command("removeRole"), Aliases("rRole", "removerole", "rrole"), RequirePermissions(Permissions.Administrator), Description("Remove a users role, it MUST include a reason at the end (you can't pass in null)."), Hidden]
        public async Task removeRole(CommandContext ctx, DiscordMember member, DiscordRole role, string reason)
        {
            await ctx.Guild.RevokeRoleAsync(member, role, reason);
            await ctx.RespondAsync($"Revoked {member.DisplayName}'s role as {role} because of ''{reason}''.");
        }

        [Command("kickUser"), Aliases("kick", "Kick"), RequirePermissions(Permissions.Administrator), Description("Kick mentioned users from the server. This does not work on users with the 'Administrator' Permission"), Hidden]
        public async Task kickUser(CommandContext ctx, DiscordMember member)
        {
            await ctx.Guild.RemoveMemberAsync(member);
            await ctx.RespondAsync($"Kicked {member.DisplayName} from {ctx.Guild}.");
        }

        [Command("banUser"), Aliases("ban", "Ban"), RequirePermissions(Permissions.Administrator), Description("Used to BAN mentioned users for the set amount of days"), Hidden]
        public async Task banUser(CommandContext ctx, DiscordMember member, int days, string reason)
        {
            await ctx.Guild.BanMemberAsync(member, days, reason);
            await ctx.RespondAsync($"Banned {member} from {ctx.Guild} for ''{reason}''. The Ban will be lifted {days} days from now.");
        }

        [Command("retrieveBans"), Aliases("findBans", "bans", "Bans", "FindBans"), Description("Lists all bans.")]
        public async Task retrieveBans(CommandContext ctx)
        {
            await ctx.RespondAsync($"List of Bans from {ctx.Guild} : ");
            await ctx.Guild.GetBansAsync();
        }

        //The mute commands currently don't work.
        [Command("muteUser"), Aliases("mute", "muteuser"), Description("Mute the mentioned user, this is for NO set time and you MUST use unmute to unmute the muted player"), RequirePermissions(Permissions.Administrator), Hidden]
        public async Task muteUser(CommandContext ctx, DiscordMember member, bool ask, string reason)
        {
            await member.SetMuteAsync(ask, reason);//Not sure how to reference the channel and the player and mute them in that.
            await ctx.RespondAsync($"Muted {member.DisplayName} for ''{reason}''");
        }

        [Command("unmuteUser"), Aliases("unmute", "unmuteuser", "nomute", "removemute", "rmute", "nmute", "omittmute", "omute"), RequirePermissions(Permissions.Administrator), Description("Unmute the mentioned user."), Hidden]
        public async Task unmuteUser(CommandContext ctx, DiscordMember member, string reason)
        {
            await member.SetMuteAsync(false, reason);//Not sure how to reference the channel and the player and unmute them in that.
            await ctx.RespondAsync($"Unmuted {member.DisplayName}.");
        }
    }
}
