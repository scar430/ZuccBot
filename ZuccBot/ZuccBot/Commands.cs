using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using ZuccBot.ZuccRPG;

namespace ZuccBot
{
    public class Commands
    {
        //Some of the commands end with deleting the message so that way you don't have constant commmand spam

        /*[Command("help")]
        public async Task help(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder() { Title = "**Help**", Description = "General help command that contains pointers to more specific help commands.", Color = DiscordColor.CornflowerBlue};

            embed.AddField("uHelp", "`uHelp` or ''Utility Help'' is used for help with all Utility commands (banning, kicking, adding/removing roles, etc.).");
            embed.AddField("rpgHelp", "`rpgHelp` or ''Role-Playig-Game Help'' is used for help with ZuccBot's RPG.");

            DiscordEmbed help = embed;

            await ctx.Message.DeleteAsync();
            await ctx.RespondAsync("", false, help);
        }*/

        /*[Command("uHelp")]
        public async Task uHelp(CommandContext ctx)
        {
            Console.WriteLine("Started utility help.");

            var embed = new DiscordEmbedBuilder() { Title = "Utility Help", Description = "Help with utility commands.", Color = DiscordColor.CornflowerBlue};

            foreach (MemberInfo member in typeof(Commands).GetMembers())
            {
                Console.WriteLine("1");
                Attribute[] attributes = Attribute.GetCustomAttributes(member);

                foreach (System.Attribute attribute in attributes)
                {
                    Console.WriteLine("2");
                    if (attribute is CommandAttribute)
                    {
                        Console.WriteLine("3");
                        CommandAttribute command = (CommandAttribute)attribute;

                        foreach (System.Attribute _attribute in attributes)
                        {
                            Console.WriteLine("4");
                            if (attribute is DescriptionAttribute)
                            {
                                Console.WriteLine("5");
                                DescriptionAttribute description = (DescriptionAttribute)attribute;
                                embed.AddField(command.Name.ToString(), description.Description.ToString());
                            }
                            else
                            {
                                Console.WriteLine("6");
                                embed.AddField(command.Name.ToString(), "No description available.");
                            }
                        }
                    }
                }
            }

            DiscordEmbed utilityEmbed = embed;

            await ctx.RespondAsync("", false, utilityEmbed);

            Console.WriteLine("Embed has sent.");

            await Task.CompletedTask;
        }*/

        [Command("log")]
        public async Task log(CommandContext ctx)
        {
            if (Program.logEnabled == false)
            {
                Program.logEnabled = true;
            }
            else
            {
                Program.logEnabled = false;
            }

            await Task.CompletedTask;
        }

        //This command only works for the owner's account (scar430) so if your trying to run this and it's not working then that's why.
        //Shut Down command
        [Command("ShutDown"), Aliases("off", "shutdown", "shutoff", "die", "death", "sleep"), RequireOwner, Hidden]
        public async Task ShutOff(CommandContext ctx)
        {
            await ctx.RespondAsync($"ZuccBot is turning off.");
            Environment.Exit(420);//huehuehuehue
            await ctx.Message.DeleteAsync();
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
            await ctx.RespondAsync($"Hey, {ctx.User.Mention}, here's a link to Columbia's website: https://www.gocolumbia.edu/");
            await ctx.Message.DeleteAsync();
        }

        /*What time did the mentioned user join
         * Command : userJoined @User
         */
        [Command("userJoined"), Aliases("joined", "join", "userJoin", "userjoin", "userjoined"), Description("What time did the mentioned user join the server?")]
        public async Task joined(CommandContext ctx, DiscordMember member)
        {
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
            await ctx.Message.DeleteAsync();
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

        /*
        [Command("createRole"), Description("Create a role.")]
        public async Task createRole(CommandContext ctx)
        {
            await ctx.Guild.CreateRoleAsync();
        }*/

        /* Grant a Role to a user.
         * Command : addRole @User @Role
         * **NOTE** The roles must be able to be mentioned by the person invoking the command, it's not just a string that's cross referenced with a list of roles.
         */
        [Command("addRole"), Aliases("role", "aRole", "addrole", "arole"), RequirePermissions(Permissions.Administrator), Description("Add a role to a player."), Hidden]
        public async Task addRole(CommandContext ctx, DiscordMember member, DiscordRole role)
        {
            await ctx.Guild.GrantRoleAsync(member, role);
            await ctx.RespondAsync($"Granted {role} to {member.DisplayName}.");
            await ctx.Message.DeleteAsync();
        }

        /* Revoke a Role from a user.
         * Command : removeRole @User @Role [reason for revoking role]
         */
        [Command("removeRole"), Aliases("rRole", "removerole", "rrole"), RequirePermissions(Permissions.Administrator), Description("Remove a users role, it MUST include a reason at the end (you can't pass in null)."), Hidden]
        public async Task removeRole(CommandContext ctx, DiscordMember member, DiscordRole role, string reason)
        {
            await ctx.Guild.RevokeRoleAsync(member, role, reason);
            await ctx.RespondAsync($"Revoked {member.DisplayName}'s role as {role} because of ''{reason}''.");
            await ctx.Message.DeleteAsync();
        }


        /* Kick a User
         * Command : kick @User
         */
        [Command("kickUser"), Aliases("kick", "Kick"), RequirePermissions(Permissions.Administrator), Description("Kick mentioned users from the server. This does not work on users with the 'Administrator' Permission"), Hidden]
        public async Task kickUser(CommandContext ctx, DiscordMember member)
        {
            await ctx.Guild.RemoveMemberAsync(member);//Removing the mentioned user from the Guild.
            await ctx.RespondAsync($"Kicked {member.DisplayName} from {ctx.Guild}.");//Just the Bot telling everyone what it is doing
            await ctx.Message.DeleteAsync();
        }

        [Command("banUser"), Aliases("ban", "Ban"), RequirePermissions(Permissions.Administrator), Description("Used to BAN mentioned users for the set amount of days"), Hidden]
        public async Task banUser(CommandContext ctx, DiscordMember member, int days, string reason)
        {
            await ctx.Guild.BanMemberAsync(member, days, reason);
            await ctx.RespondAsync($"Banned {member} from {ctx.Guild} for ''{reason}''. The Ban will be lifted {days} days from now.");
            await ctx.Message.DeleteAsync();
        }

        [Command("retrieveBans"), Aliases("findBans", "bans", "Bans", "FindBans"), Description("Lists all bans.")]
        public async Task retrieveBans(CommandContext ctx)
        {
            await ctx.RespondAsync($"List of Bans from {ctx.Guild} : ");//Just the bot telling everyone what it is posting.
            await ctx.Guild.GetBansAsync();
            await ctx.Message.DeleteAsync();
        }

        //Due to a problem with connecting a mentioned user to a channel and muting them from that channel, these mute commands do not work
        //Broken
        [Command("muteUser"), Aliases("mute", "muteuser"), Description("Mute the mentioned user, this is for NO set time and you MUST use unmute to unmute the muted player"), RequirePermissions(Permissions.Administrator), Hidden]
        public async Task muteUser(CommandContext ctx, DiscordMember member, string reason)
        {
            await member.SetMuteAsync(true, reason);//This is incorrect because I only mentioned the user and not in what channel they are being mute.
            await ctx.RespondAsync($"Muted {member.DisplayName} for '{reason}'");//Just the bot telling everyone what it's doing.
            await ctx.Message.DeleteAsync();
        }

        //Broken
        [Command("unmuteUser"), Aliases("unmute", "unmuteuser", "nomute", "removemute", "rmute", "nmute", "omittmute", "omute"), RequirePermissions(Permissions.Administrator), Description("Unmute the mentioned user."), Hidden]
        public async Task unmuteUser(CommandContext ctx, DiscordMember member, string reason)
        {
            await member.SetMuteAsync(false, reason);//Not sure how to reference the channel and the player and unmute them in that.
            await ctx.RespondAsync($"Unmuted {member.DisplayName}.");//Just the bot telling everyone what it's doing.
            await ctx.Message.DeleteAsync();
        }
    }
}
