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
using ZuccBot.UI;

namespace ZuccBot.UI
{
    public class PaginatedEmbed
    {
        public IEnumerable<EmbedPage> pages;
        public DiscordMessage message;
        public DiscordEmoji[] selections;

        public PaginatedEmbed(string title, string description, DiscordEmoji[] _selections, IEnumerable<EmbedPage> _pages)
        {
            selections = _selections;
            pages = _pages;//In most cases you can parse in null, in other cases your probably copying pages over.
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

    //Associating elements with an embed (this can probably be done with a dictionary however I'm keeping it in case I need to add anything to it.)
    public class EmbedPage
    {
        public DiscordEmbed embed;
        public PageElement[] elements;

        public EmbedPage(DiscordEmbed _embed, PageElement[] _elements)
        {
            embed = _embed;
            elements = _elements;
        }
    }

    public class PageElement
    {
        public string header;
        public string content;

        public PageElement(string _header, string _content)
        {
            header = _header;
            content = _content;
        }
    }
}
