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
using CheckerLib;
namespace NitroSniper

{
    class Program
    {
        public static DiscordSocketClient Client = new DiscordSocketClient();
        public static int gifts = 0;

        static void Main(string[] args)
        {
            Checker.TryAuth();
            Console.Title = "Discord Nitro Sniper | Gifts Tested: " + gifts +" | Made for Cracked.to | By Gaztoof#9769";
            DiscordSocketClient client = new DiscordSocketClient();
            string token = null;
            try
            {
                token = File.ReadAllText("token.txt");
            }
            catch
            {
                Console.WriteLine("Please, enter your token:");
                token = Console.ReadLine();
            }
            begin:
            try
            {
                client.Login(token);
                Console.WriteLine("Successfully logged in!", Color.LightGreen);
                File.Delete("token.txt");
                File.AppendAllText("token.txt", token);
            }
            catch
            {
                Console.WriteLine("[ERROR] Your token might be wrong.", Color.IndianRed);
                Console.WriteLine("Please, enter your token:");
                token = Console.ReadLine();
                File.Delete("token.txt");
                File.AppendAllText("token.txt", token);
                goto begin;
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
                    if (match.ToString().Length != 29 && match.ToString().Length != 37)
                    {
                        return;
                    }

                    if (args.Message.Content.ToLower().Contains("discord.gift"))
                    {
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        Console.Write("[" + args.Message.Author + "] ", Color.Yellow);
                        if (args.Message.GuildId != null)
                        {
                            Console.Write("[" + args.Message.GuildId + "] ", Color.Orange);
                        }
                        else
                        {
                            Console.Write("[DM] ", Color.Orange);

                        }
                        try
                        {
                            client.RedeemNitroGift(match.ToString().Replace("discord.gift/", ""), args.Message.ChannelId);
                            Console.Write("[CLAIMED] ", Color.LightGreen);

                        }
                        catch (DiscordHttpException ex)
                        {
                            if (ex.ToString().Contains("Unknown Gift Code"))
                            {

                                Console.Write("[FAIL] ", Color.IndianRed);
                            }
                            else if (ex.ToString().Contains(""))
                            {

                                Console.Write("[ALREADY REDEMEED] ", Color.LightBlue);
                            }
                            else
                            {
                                Console.Write("[FAIL] ", Color.IndianRed);
                            }
                        }

                        Console.Write(watch.ElapsedMilliseconds / 1000 + "." + watch.ElapsedMilliseconds + "s ");
                        watch.Stop();
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
