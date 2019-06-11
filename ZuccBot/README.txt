GENERAL
- This goes without being said, all the code is wonky however it is usable, everything does have some basic functionality to it and if not some then all.

PACKAGES (Go to Properties>NuGet Packages (2nd from the bottom?)>Browse>Search for what you want)
- DSharpPlus (And all of it's children, if you are missing a minor package from this it should suggest using it? That goes for all packages)
- Newtonsoft.Json (And all of it's children)
- **NOTE FOR JOE** I downloaded it onto a new computer at school and it came with all it's normal packages, I would try checking that all the normal packages are there before you run it?

ADDING ZUCCBOT:
- ZuccBot should be invited with level 8 permissions to the guild.
- Make sure ZuccBot is turned ON when you ADD him to your guild, a little greeter message should pop up.

GENERIC RPG
- Saving to disc does NOT work, as soon as you shut off the RPG it will lose all progress (Due to a problem with DiscordUser class, look for Task rpgInit() in Prgram.cs, there's some notes on there. It is fixable but hasn't been done yet.)
- There are some weird glitches when multiple people interact with one thing (e.g. Multiple people messing around with a New Character sheet or People attacking each other, problem's like this can be fixed with more time.).

VS IDE PROPERTIES
- Target Framework should be 2.0
- Startup Object should be ZuccBot.Program
- Output Type should be Console Application

OTHER NOTES
- In the RPG, by default your character starts in Level_1

**NOTE FOR JOE**
That should be it, if you have any problems you obviously know to just contact me on discord, if this becomes to much of a hassel on your end I could just run it on my end and you could play around with it.

