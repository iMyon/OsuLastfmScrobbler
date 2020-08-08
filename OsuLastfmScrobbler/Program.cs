using System;
using System.Diagnostics;
using System.IO;
using OsuLastfmScrobbler.client;

namespace OsuLastfmScrobbler
{
    internal class Program
    {
        private static string APP_NAME = "OsuLastfmScrobbler";
        
        public static void Main(string[] args)
        {
            Console.Title = APP_NAME;
            if (IsRunning())
            {
                // just start osu when there is another program running
                StartOsu();
                return;
            }
            var scrobbler = new OsuScrobbler();
            scrobbler.Start();
            scrobbler.OnStartedChanged += StartOsu;

            // hang the process
            while (true)
            {
                Console.ReadLine();
            }
        }

        static void StartOsu()
        {
            var osuPath = Path.GetFullPath(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\osu!.exe"));
            try
            {
                System.Diagnostics.Process.Start(osuPath);
            }
            catch (Exception e)
            {

                Console.WriteLine(
                    $"Trying to open osu!.exe, but cannot found the file at '{osuPath}', you can open Osu by yourself.");
            }
        }

        static bool IsRunning()
        {
            Process cur = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName==$"{APP_NAME}" && p.Id != cur.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}