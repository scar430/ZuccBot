using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using ZuccBot.ZuccRPG.Generic;
using System.Collections;
using DSharpPlus.Net;

namespace ZuccBot.UI
{
    public class PaginatedEmbed
    {
        public IEnumerable<EmbedPage> pages;
        public DiscordEmbed embed;
        public CommandContext ctx;
        public DiscordMessage message;
        public DiscordEmoji[] selections;

        public PaginatedEmbed(string title, string description, CommandContext _ctx, DiscordEmoji[] _selections, IEnumerable<EmbedPage> _pages, DiscordMessage _message)
        {
            embed = new DiscordEmbedBuilder() { Title = title, Description = description };
            ctx = _ctx;
            selections = _selections;
            pages = _pages;//In most cases you can parse in null, in other cases your probably copying pages over.
            message = _message;
        }

        public bool GetSelect(DiscordEmoji emoji)
        {
            if (message.Reactions.Max().Emoji.Name == emoji.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class EmbedPage
    {
        DiscordEmbed embed;
        PageElement[] elements;

        EmbedPage(DiscordEmbed _embed)
        {
            embed = _embed;
        }
    }

    public class PageElement
    {
        EmbedPage embedPage;
        string header;
        string[] content;

        PageElement(string _header, string[] _content)
        {
            header = _header;
            content = _content;
        }
    }
}
