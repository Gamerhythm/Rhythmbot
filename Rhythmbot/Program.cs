using System;
using System.Linq;
using System.Text;
using Discord;
using Discord.Commands;
using System.Diagnostics;
using System.Net;
using RestSharp.Extensions.MonoHttp;
using System.Xml;
using System.IO;
using System.Timers;

namespace Rhythmbot
{
    class Program
    {
        StringBuilder sb = new StringBuilder();
        string BuildDate = "29/01/17";
        Stopwatch ApplicationOpen = Stopwatch.StartNew();
        Random rand;
        static void Main(string[] args)
        {
            new Program().Start();
        }


        string[] DogeLocations;

        private DiscordClient _client;

        public void Start()
        {
            _client = new DiscordClient(x =>
            {
                x.AppName = "Rhythmbot";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            _client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = false;
            });

            System.IO.StreamReader tokeninfo = new System.IO.StreamReader("token.txt");
            var token = tokeninfo.ReadLine();

            CreateCommands();
            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(token, TokenType.Bot);
                _client.SetGame("with servers!");
            });
        }


        public void CreateCommands()
        {
            rand = new Random();
            DogeLocations = new string[]
            {
             "doge1.jpg",
             "doge2.jpg",
             "doge3.png",
             "doge4.jpg",
             "doge5.png",
             "doge6.jpg",
             "doge7.png",
             "doge8.gif"
            };
            var cService = _client.GetService<CommandService>();
            cService.CreateCommand("shorten")
                .Parameter("shortenunparsed", ParameterType.Unparsed)
.Do(async (e) =>
{
    string shortUrl;
    using (WebClient wb = new WebClient())
    {

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(wb.DownloadString("http://api.bitly.com/v3/shorten/?login=topkek69&apiKey=R_18d57b9b98704b7ead5a9e7ebf79b189&longUrl=" + e.GetArg("shortenunparsed") + "&format=xml"));

        shortUrl = xmlDoc.GetElementsByTagName("url")[0].InnerText;

    }
    await e.Channel.SendMessage(e.User.Mention + "Your shortened URL is: " + shortUrl);
});
            cService.CreateCommand("donate")
.Do(async (e) =>
{
    await e.Channel.SendMessage(e.User.Mention + " Rhythmbot Premium is only $5. Please join https://discord.gg/pkwgK6X and chat to Gamerhythm for more info.");
});
            cService.CreateCommand("triggered")
.Do(async (e) =>
{
    await e.Channel.SendFile("triggered.gif");
});
            cService.CreateCommand("createrole")
.Parameter("roletoadd", ParameterType.Unparsed)
.Do(async (e) =>
{
    Boolean ispvt = e.Channel.IsPrivate;
    string ServerIDChecker;
    if (ispvt == false)
    {
        ServerIDChecker = e.Server.Id.ToString();
    }
    else
    {
        ServerIDChecker = "69696969696969";
    }
    string VIPUsers = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\vipusers.txt");
    string VIPServers = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\vipservers.txt");
    if ((VIPUsers.Contains(e.User.Mention)) | (VIPServers.Contains(ServerIDChecker)))
    {
        if (ispvt == true)
        {
            await e.Channel.SendMessage(e.User.Mention + " You need to be in a server with role managment permissions for this command to work.");
       } else {
            if (e.User.ServerPermissions.ManageRoles == true) {
                try {
                    await e.Server.CreateRole(e.GetArg("roletoadd"));
                    await e.Channel.SendMessage(e.User.Mention + " The role (" + e.GetArg("roletoadd") + ") was sucessfully added.");
                } catch {
                    await e.Channel.SendMessage(e.User.Mention + " There was an error adding the rank. Check the bot has permission.");
                }
            } else {
                await e.Channel.SendMessage(e.User.Mention + " You need to be in a server with role managment permissions for this command to work.");
            } 
       }
    } else {
        await e.Channel.SendMessage(e.User.Mention + " You require Rhythmbot Premium which you can buy for $5. Do `!donate` for more info.");
    }
});
cService.CreateCommand("support")
.Do(async (e) =>
{
    await e.Channel.SendMessage(e.User.Mention + " https://www.youtube.com/watch?v=PtXtIivRRKQ");
});
            cService.CreateCommand("bottechnical")
.Do(async (e) =>
{
Boolean ispvt = e.Channel.IsPrivate;
    string ServerIDChecker;
    if (ispvt == false) {
         ServerIDChecker = e.Server.Id.ToString();
    } else {
         ServerIDChecker = "69696969696969";
    }
string BotType;
string VIPUsers = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\vipusers.txt");
string VIPServers = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\vipservers.txt");
    if ((VIPUsers.Contains(e.User.Mention)) | (VIPServers.Contains(ServerIDChecker)))
    {
        BotType = "Rhythmbot Premium";
    } else {
        BotType = "Rhythmbot Free";
    }
        string BotUptime = ApplicationOpen.Elapsed.ToString("dd\\:hh\\:mm\\:ss");
    await e.Channel.SendMessage(e.User.Mention + @"
BOT UPTIME: " + BotUptime + @"
BOT BUILDDATE: " + BuildDate + @"
SERVERS: " + _client.Servers.Count() + @"
TYPE: " + BotType);
});
            cService.CreateCommand("lmgtfy")
.Parameter("lmgtfyterm", ParameterType.Unparsed)
.Do(async (e) =>
{
    string LMGTFYURLTerm = $"{e.GetArg("lmgtfyterm")}";
    String FinalLMGTFY = System.Uri.EscapeDataString(LMGTFYURLTerm);
    await e.Channel.SendMessage("http://lmgtfy.com/?iie=1&q=" + FinalLMGTFY);
});
            cService.CreateCommand("purge")
.Parameter("purgenumber", ParameterType.Unparsed)
.Do(async (e) =>
{
    Boolean ispvt = e.Channel.IsPrivate;
    int purgeamountparsed = Int32.Parse(e.GetArg("purgenumber")) + 1;
    Message[] messages2delete = new Message[purgeamountparsed];
    messages2delete = e.Channel.DownloadMessages(purgeamountparsed).Result;
    if (ispvt == true)
    {
        await e.Channel.SendMessage(e.User.Mention + " This command does not work in private messages.");
    }
    else
    {
        if (e.User.ServerPermissions.ManageMessages == true) 
        {
            try
            {
                await e.Channel.DeleteMessages(messages2delete);
                await e.Channel.SendMessage(e.User.Mention + " " + (purgeamountparsed - 1) + " messages purged!");
            }
            catch
            {
                await e.Channel.SendMessage(e.User.Mention + " There was an error purging the messages. Check the bot has permission and the amount of messages you are trying to purge is not greater than the amount on the server.");
            }
        }
        else
        {
            await e.Channel.SendMessage(e.User.Mention + " You do not have permission to purge messages.");
        }
    }
});
            cService.CreateCommand("stacksearch")
.Parameter("stackoverflow", ParameterType.Unparsed)
.Do(async (e) =>
{
    string StackURLTerm = $"{e.GetArg("stackoverflow")}";
    String FinalStack = System.Uri.EscapeDataString(StackURLTerm);
    await e.Channel.SendMessage(e.User.Mention + " " + "http://stackoverflow.com/search?q=" + FinalStack);
});
            cService.CreateCommand("whois")
.Parameter("ip", ParameterType.Unparsed)
.Do(async (e) =>
{
    var whoislink = $"https://who.is/whois-ip/ip-address/{e.GetArg("ip")}";
    await e.Channel.SendMessage(e.User.Mention + " " + whoislink);
});
            cService.CreateCommand("uktime")
.Do(async (e) =>
{
    string UkTime = DateTime.Now.ToString("HH:mm");
    await e.Channel.SendMessage(e.User.Mention + " The time is currently " + UkTime + " in the UK.");
});
            cService.CreateCommand("ustime")
.Do(async (e) =>
{
    string CurrentUSMin = DateTime.Now.ToString("mm");
    int CurrentUKTime = Int32.Parse(DateTime.Now.ToString("HH"));
    int CurrentUSTime;
    CurrentUSTime = CurrentUKTime - 5;
    if (CurrentUSTime == -5)
    {
        CurrentUSTime = 19;
    }
    if (CurrentUSTime == -4)
    {
        CurrentUSTime = 20;
    }
    if (CurrentUSTime == -3)
    {
        CurrentUSTime = 21;
    }
    if (CurrentUSTime == -4)
    {
        CurrentUSTime = 22;
    }
    if (CurrentUSTime == -3)
    {
        CurrentUSTime = 23;
    }
    if (CurrentUSTime == -2)
    {
        CurrentUSTime = 00;
    }
    if (CurrentUSTime == -1)
    {
        CurrentUSTime = 01;
    }
    await e.Channel.SendMessage(e.User.Mention + " The time is currently " + CurrentUSTime + ":" + CurrentUSMin + " in New York, USA.");
});
            cService.CreateCommand("doge")
.Do(async (e) =>
{
    int DogeRandom = rand.Next(DogeLocations.Length);
    string DogePicture = DogeLocations[DogeRandom];
    await e.Channel.SendFile(DogePicture);
});
            cService.CreateCommand("learnc#")
    .Do(async (e) =>
    {
        await e.Channel.SendMessage(e.User.Mention + " http://www.learncs.org");
    });
            cService.CreateCommand("learnc++")
.Do(async (e) =>
{
    await e.Channel.SendMessage(e.User.Mention + " http://www.learncpp.com");
});
            cService.CreateCommand("learnjava")
    .Do(async (e) =>
    {
        await e.Channel.SendMessage(e.User.Mention + " https://www.learnjavaonline.org");
    });
            cService.CreateCommand("learnpython")
.Do(async (e) =>
{
    await e.Channel.SendMessage(e.User.Mention + " https://www.learnpython.org");
});
            cService.CreateCommand("learnscratch")
.Do(async (e) =>
{
    await e.Channel.SendMessage(e.User.Mention + " I think/hope you are joking.");
});
            cService.CreateCommand("add")
.Parameter("numbers2add", ParameterType.Unparsed)
.Do(async (e) =>
{
    string[] numbersaddraw2 = e.GetArg("numbers2add").Split();
    int number1addraw = Int32.Parse(numbersaddraw2[0]);
    int number2addraw = Int32.Parse(numbersaddraw2[1]);
    int numbersaddedtogether = number1addraw + number2addraw;
    await e.Channel.SendMessage(e.User.Mention + " " + number1addraw.ToString() + " and " + number2addraw.ToString() + " added together makes " + numbersaddedtogether.ToString());
});
            cService.CreateCommand("multiply")
.Parameter("numbers2times", ParameterType.Unparsed)
.Do(async (e) =>
{
    string[] numberstimesraw2 = e.GetArg("numbers2times").Split();
    int number1timesraw = Int32.Parse(numberstimesraw2[0]);
    int number2timesraw = Int32.Parse(numberstimesraw2[1]);
    int numberstimestogether = number1timesraw * number2timesraw;
    await e.Channel.SendMessage(e.User.Mention + " " + number1timesraw.ToString() + " and " + number2timesraw.ToString() + " mutiplied together makes " + numberstimestogether.ToString());
});
            cService.CreateCommand("divide")
.Parameter("numbers2divide", ParameterType.Unparsed)
.Do(async (e) =>
{
    string[] numbersdivideraw2 = e.GetArg("numbers2divide").Split();
    int number1divideraw = Int32.Parse(numbersdivideraw2[0]);
    int number2divideraw = Int32.Parse(numbersdivideraw2[1]);
    int numbersdividetogether = number1divideraw / number2divideraw;
    await e.Channel.SendMessage(e.User.Mention + " " + number1divideraw.ToString() + " divided by " + number2divideraw.ToString() + " makes " + numbersdividetogether.ToString());
});
            cService.CreateCommand("help")
.Do(async (e) =>
{
    await e.Channel.SendMessage(e.User.Mention + " Check your DM's");
    await e.User.SendMessage(@"
`!createrole [role to create]` - **Requires either you or the server have Rhythmbot Premium, and also that you and the bot have the Manage Roles permission.** Allows you to easily create a role.

`!purge [amount to purge]` - **Requires that you and the bot have the Manage Messages permission.** Allows you to easily purge messages.

`!shorten [link to shorten]` - Shortens a link via the bit.ly API.

`!support` - Sends a video of the IT robot from The IT Crowd in the chat you are in.

`!donate` -Takes you to a patreon where you can donate to me monthly. I love all donations greatly as they help support this project.

`!stacksearch [search term]` - Brings up the search page for Stack Overflow preprepared with your question.

`!whois [IP address]` - Generates a link to the who.is of a typed IP. Please note on most servers to DM the bot as some servers like TCDG and SSL punish for active IP's.

`!doge` - Fetches a random picture of a doge from my server, will add more pictures to the bot when I have nothing better to do. The bot needs SendFile privileges from the server for this to work.

`!learnjava` - Links to a tutorial about learning Java.

`!learnc++` - Links to a tutorial about learning C++.

`!learnpython` - Links to a tutorial about learning Python.

`!learnc#` - Links to a tutorial about learning C#.

`!uktime` - Shows time in the UK.

`!ustime` - Shows time in the US.

`!add [first number] [second number]` - Adds 2 numbers together.

`!multiply [first number] [second number]` - Multiplys 2 numbers together.

`!divide [number you want divided] [number you want it divided by]` - Allows you to divide a number.

`!bottechnical` - Shows bot technical information. (such as uptime and the bot's builddate)

`!triggered` - Shows the Triggered GIF.

That is it, I will probably add more commands since I am regularly developing the bot. Remember to remove the [] tags when running the commands since they are only there for reference.");
});
        }


        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
            // [INFO] [SOURCE OF ERROR] [ERROR]
        }
    }
}