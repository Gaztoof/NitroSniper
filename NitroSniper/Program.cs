using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;
using Console = Colorful.Console;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
//using CheckerLib;
using System.Windows.Forms;

namespace NitroSniper

{
    class Program
    {
        public static DiscordSocketClient Client = new DiscordSocketClient();
        public static int gifts = 0;
        public static string AESKey = "Modify this to whatever you want, or just make it non-hardcoded...";

        public static string getToken()
        {
            File.WriteAllText(Application.StartupPath + "\\" + "token", "");
            string token = "";
            while (true)
            {
                Console.Write("Please, enter your token: ", Color.White);
                token = Console.ReadLine();
                try
                {
                    Client.Login(token);
                    break;
                }
                catch {
                    Console.WriteLine("The token you've entered is wrong.", Color.IndianRed);
                }
            }
            File.WriteAllBytes(Application.StartupPath + "\\" + "token", Security.Encrypt(token, AESKey, Security.GetFileHash(Application.ExecutablePath)));
            return token;
        }

        static void Main(string[] args)
        {
            //Checker.TryAuth();
            // token encryption will be different every time you compile, since the AES encryption's salt is the md5 checksum of the .Exe
            Console.Title = "Discord Nitro Sniper | Gifts Tested: " + gifts +" | Made for Cracked.to | By Gaztoof#9769";
            DiscordSocketClient client = new DiscordSocketClient();
            string token = null;
            while(true)
            {
                try
                {
                    if (File.Exists(Application.StartupPath + "\\" + "token") && File.ReadAllText(Application.StartupPath + "\\" + "token") != "" && File.ReadAllBytes(Application.StartupPath + "\\" + "token").Length == 64)
                    {
                        token = Security.Decrypt(File.ReadAllBytes(Application.StartupPath + "\\" + "token"), AESKey, Security.GetFileHash(Application.ExecutablePath));
                        client.Login(token);
                        break;
                    }
                    else
                    {
                        token = getToken();
                    }
                }
                catch {
                    File.WriteAllText(Application.StartupPath + "\\" + "token", "");
                    Console.WriteLine("The saved token expired!", Color.IndianRed);
                }
            }
            Console.WriteLine(@"
███╗   ██╗██╗████████╗██████╗  ██████╗ 
████╗  ██║██║╚══██╔══╝██╔══██╗██╔═══██╗
██╔██╗ ██║██║   ██║   ██████╔╝██║   ██║
██║╚██╗██║██║   ██║   ██╔══██╗██║   ██║
██║ ╚████║██║   ██║   ██║  ██║╚██████╔╝
╚═╝  ╚═══╝╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ 
                                       ",Color.FromArgb(49, 146, 255));

            client.OnMessageReceived += OnMessageReceived;

            System.Threading.Thread.Sleep(-1);

        }
        private static void OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {

            Console.Title = "Discord Nitro Sniper | Gifts Tested: " + gifts + " | Made for Cracked.to | By Gaztoof#9769";
            var regex = new Regex(@"discord.gift\\?.*");
            foreach (var match in regex.Matches(args.Message.Content))
            {
                try
                {
                    if (args.Message.Content.ToLower().Contains("discord.gift"))
                    {
                        Stopwatch watch = new Stopwatch();
                        watch.Start();

                        if (match.ToString().Length != 29 && match.ToString().Length != 37)
                        {
                            Console.Write("[" + args.Message.Author + "] ", Color.Yellow);

                            if (args.Message.GuildId.HasValue)
                                    Console.Write("[" + client.GetGuild(args.Message.GuildId.Value).Name + "] ", Color.Orange);
                            else
                                Console.Write("[DM] ", Color.Orange);

                            Console.Write("[FAKE] ", Color.AliceBlue);
                            goto End;
                        }

                        try
                        {
                            client.RedeemNitroGift(match.ToString().Replace("discord.gift/", ""), args.Message.ChannelId);
                            watch.Stop();

                            Console.Write("[" + args.Message.Author + "] ", Color.Yellow);
                            if (args.Message.GuildId.HasValue)
                                Console.Write("[" + client.GetGuild(args.Message.GuildId.Value).Name + "] ", Color.Orange);
                            else
                                Console.Write("[DM] ", Color.Orange);
                            Console.Write("[CLAIMED] ", Color.LightGreen);

                        } // duplicate is being done for performance reasons
                        catch (DiscordHttpException ex)
                        {
                            watch.Stop();
                            string exceptionMessage = ex.ToString();
                            
                            Console.Write("[" + args.Message.Author + "] ", Color.Yellow);

                            if (args.Message.GuildId.HasValue && args.Message.GuildId != null)
                                Console.Write("[" + client.GetGuild(args.Message.GuildId.Value).Name + "] ", Color.Orange);
                            else if (args.Message.GuildId != null)
                                Console.Write("[" + args.Message.GuildId + "] ", Color.Orange);
                            else
                                Console.Write("[DM] ", Color.Orange);

                            if (exceptionMessage.Contains("Unknown Gift Code"))
                            {
                                Console.Write("[FAIL] ", Color.IndianRed);
                            }
                            else if (exceptionMessage.Contains(""))
                            {

                                Console.Write("[ALREADY REDEMEED] ", Color.LightBlue);
                            }
                            else
                            {
                                Console.Write("[FAIL] ", Color.IndianRed);
                            }
                        }
                    End:
                        watch.Stop();
                        Console.Write(watch.ElapsedMilliseconds / 1000 + "." + watch.ElapsedMilliseconds + "s ");
                        Console.Write(match + "\n", Color.FromArgb(49, 146, 255));
                        gifts++;
                    }
                }
                catch
                {
                    Console.WriteLine("Oopsie, you're being rate limited!", Color.IndianRed);
                }
            }
        }
    }
}
