using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bludiste
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze();
            Menu menu = new Menu();
            ConsoleKeyInfo ck;
            //open menu
            menu.Main();
            //map setting
            maze.MapAction(3, menu.selectedMapsPosition);
            do {
                // map generation
                Console.Clear();
                maze.MapAction(1);
                //player input
                ck = Console.ReadKey(true);
                if (Config.debug) Console.WriteLine($"{ck.KeyChar} ");
                maze.Controler(ck);
            } while (ck.Key != ConsoleKey.Escape);
        }
    }

    public class Menu
    {
        public int selectedMainPosition, selectedMapsPosition, selectedOptionsPosition;
        public bool select, ret;
        public Menu() {
            selectedMainPosition = 1;
            selectedMapsPosition = 1;
            selectedOptionsPosition = 1;
            select = false;
        }
        public void Main()
        {
            ConsoleKeyInfo ck;
            //Main menu
            do {
                Console.Clear();
                Console.WriteLine(@"▀█████████▄   ▄█       ███    █▄  ████████▄   ▄█     ▄████████     ███        ▄████████ 
  ███    ███ ███       ███    ███ ███   ▀███ ███    ███    ███ ▀█████████▄   ███    ███ 
  ███    ███ ███       ███    ███ ███    ███ ███▌   ███    █▀     ▀███▀▀██   ███    █▀  
 ▄███▄▄▄██▀  ███       ███    ███ ███    ███ ███▌   ███            ███   ▀  ▄███▄▄▄     
▀▀███▀▀▀██▄  ███       ███    ███ ███    ███ ███▌ ▀███████████     ███     ▀▀███▀▀▀     
  ███    ██▄ ███       ███    ███ ███    ███ ███           ███     ███       ███    █▄  
  ███    ███ ███▌    ▄ ███    ███ ███   ▄███ ███     ▄█    ███     ███       ███    ███ 
▄█████████▀  █████▄▄██ ████████▀  ████████▀  █▀    ▄████████▀     ▄████▀     ██████████ 
             ▀                                                                          "+"\n");
                MainOptions();
                ck = Console.ReadKey(true);
                if (Config.debug) Console.WriteLine($"{ck.KeyChar} ");
                MainMenuControler(ck);
                ret = false;
                if (select) {
                    select = false;
                    switch (selectedMainPosition) {
                        case 1: return;
                        case 2: Maps(); break;
                        case 3: Options(); break;
                        case 4: System.Environment.Exit(1); break;
                    }
                }
            } while (true);
        }
        // maps menu
        public void Maps() {   
            ConsoleKeyInfo ck;
            if (selectedMapsPosition==0) selectedMapsPosition=1;
            do {
                Console.Clear();
                MapsOptions();
                ck = Console.ReadKey(true);
                if (Config.debug) Console.WriteLine($"{ck.KeyChar} ");
                MapsMenuControler(ck);
                if (select) {
                    select = false;
                    switch (selectedMapsPosition) {
                        case 0: return; 
                        case 1: selectedMapsPosition = 1; break;
                        case 2: selectedMapsPosition = 2; break;
                        case 3: selectedMapsPosition = 3; break;
                    }
                }
            } while (true);    
        }
        private void Options()
        {
            if (selectedOptionsPosition==0) selectedOptionsPosition=1;
            ConsoleKeyInfo ck;
            do {
                Console.Clear();
                OptionOptions();
                ck = Console.ReadKey(true);
                if (Config.debug) Console.WriteLine($"{ck.KeyChar} ");
                OptionsMenuControler(ck);
                if (select) {
                    select = false;
                    switch (selectedOptionsPosition) {
                        case 0: return;
                        case 1: Config.mapSize++; break;
                        case 2: Config.mapSize--; break;
                    }
                }
            } while (true);  
        }

        private void MainOptions() {
            if (selectedMainPosition == 1) Console.Write("> "); Console.WriteLine("Play");
            if (selectedMainPosition == 2) Console.Write("> "); Console.WriteLine("Select Map");
            if (selectedMainPosition == 3) Console.Write("> "); Console.WriteLine("Option");
            if (selectedMainPosition == 4) Console.Write("> "); Console.WriteLine("Exit");
        }
        private void MapsOptions() {
            if (selectedMapsPosition == 1) Console.Write("> "); Console.WriteLine("Generated");
            if (selectedMapsPosition == 2) Console.Write("> "); Console.WriteLine("Mapa Les");
            if (selectedMapsPosition == 3) Console.Write("> "); Console.WriteLine("Mapa Walls");
        }
        private void OptionOptions() {
            if (selectedOptionsPosition == 1) Console.Write("> "); Console.WriteLine($"Bigger generated map size ({Config.mapSize})");
            if (selectedOptionsPosition == 2) Console.Write("> "); Console.WriteLine($"Smaller generated map size ({Config.mapSize})");
        }
        internal void MainMenuControler(ConsoleKeyInfo ck) 
        {
            void Up() { if (selectedMainPosition!=1) selectedMainPosition--; }
            void Down() { if (selectedMainPosition!=4) selectedMainPosition++; }
            void Right() { select = true; }
            switch (ck.Key) {
                case ConsoleKey.W: Up(); break;
                case ConsoleKey.UpArrow: Up(); break;
                case ConsoleKey.S: Down(); break;
                case ConsoleKey.DownArrow: Down(); break;
                case ConsoleKey.D: Right(); break;
                case ConsoleKey.RightArrow: Right(); break;
                case ConsoleKey.Enter: Right(); break;
            }
        }
        internal void MapsMenuControler(ConsoleKeyInfo ck)
        {
            void Up() { if (selectedMapsPosition!=1) selectedMapsPosition--; }
            void Down() { if (selectedMapsPosition!=3) selectedMapsPosition++; }
            void Right() { select = true; }
            void Left() { select = true; selectedMapsPosition = 0; }
            switch (ck.Key) {
                case ConsoleKey.W: Up(); break;
                case ConsoleKey.UpArrow: Up(); break;
                case ConsoleKey.S: Down(); break;
                case ConsoleKey.DownArrow: Down(); break;
                case ConsoleKey.D: Right(); break;
                case ConsoleKey.RightArrow: Right(); break;
                case ConsoleKey.Enter: Right(); break;
                case ConsoleKey.A: Left(); break;
                case ConsoleKey.LeftArrow: Left(); break;
            }
        }
        private void OptionsMenuControler(ConsoleKeyInfo ck)
        {
            void Up() { if (selectedOptionsPosition!=1) selectedOptionsPosition--; }
            void Down() { if (selectedOptionsPosition!=2) selectedOptionsPosition++; }
            void Right() { select = true; }
            void Left() { select = true; selectedOptionsPosition = 0; }
            switch (ck.Key) {
                case ConsoleKey.W: Up(); break;
                case ConsoleKey.UpArrow: Up(); break;
                case ConsoleKey.S: Down(); break;
                case ConsoleKey.DownArrow: Down(); break;
                case ConsoleKey.D: Right(); break;
                case ConsoleKey.RightArrow: Right(); break;
                case ConsoleKey.Enter: Right(); break;
                case ConsoleKey.A: Left(); break;
                case ConsoleKey.LeftArrow: Left(); break;
            }
        }
    }

    public static class Config 
    {
        public static int mapSize = 10;
        public static bool generation = false;
        public static bool debug = false;
    } 
    static class Maps
    {
        /*
         * 0 = free
         * 1 = wall
         * 2 = player start pos
         * 3 = player goal pos
         */

        public static int[,] Mapa_les =
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 2, 0, 0, 0, 0, 3, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        public static int[] Mapa_les_Player_Pos = { 5, 2 };
        public static int[,] Mapa_walls =
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 2, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 3, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        public static int[] Mapa_walls_Player_Pos = { 1, 1 };
    }
    class Maze
    {
        public int[,] Map;
        public int[] playerPos;
        public Maze()
        {
            Map = new int[Config.mapSize, Config.mapSize];
            playerPos = new int[2];
        } 

        internal void Controler(ConsoleKeyInfo ck)
        {
            void Up() { this.MapAction(2, 1); }
            void Down() { this.MapAction(2, 2); }
            void Right() { this.MapAction(2, 3); }
            void Left() { this.MapAction(2, 4); }
            switch (ck.Key) {
                case ConsoleKey.W: Up(); break;
                case ConsoleKey.UpArrow: Up(); break;
                case ConsoleKey.S: Down(); break;
                case ConsoleKey.DownArrow: Down(); break;
                case ConsoleKey.D: Right(); break;
                case ConsoleKey.RightArrow: Right(); break;
                case ConsoleKey.A: Left(); break;
                case ConsoleKey.LeftArrow: Left(); break;
            }
        }
        public void MapAction(int action, int value = 0)
        { try {
            switch (action)
                {
                    //1. display
                    case 1:
                        MapDisplay();
                        break;
                    //2. player move
                    case 2:
                        if ((int?)Map.GetValue(playerPos[0], playerPos[1]) == 2)
                        switch (value)
                        {
                            //1. North/UP
                            //2. South/Down
                            //3. East/Right
                            //4. West/Left
                            case 1:
                                if ((int?)Map.GetValue(playerPos[0] - 1, playerPos[1]) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[0]--; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0] - 1, playerPos[1]) == 3) VictoryScrean();
                                else throw new Exception("Invalid move");
                                break;
                            case 2:
                                if ((int?)Map.GetValue(playerPos[0] + 1, playerPos[1]) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[0]++; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0] + 1, playerPos[1]) == 3) VictoryScrean();
                                else throw new Exception("Invalid move");
                                break;
                            case 3:
                                if ((int?)Map.GetValue(playerPos[0], playerPos[1] + 1) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[1]++; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0], playerPos[1] + 1) == 3) VictoryScrean();
                                else throw new Exception("Invalid move");
                                break;
                            case 4:
                                if ((int?)Map.GetValue(playerPos[0], playerPos[1] - 1) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[1]--; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0], playerPos[1] - 1) == 3) VictoryScrean();
                                else throw new Exception("Invalid move");
                                break;
                            default:
                                throw new Exception("Invalid value");
                        }
                        else throw new Exception("Player async error");
                        break;

                    //3. mapSet
                    case 3:
                        switch (value)
                        {
                            //1. Generated
                            case 1:
                                Map = Maps.Mapa_les;
                                playerPos = Maps.Mapa_les_Player_Pos;
                                break;
                            //2. MapaLes
                            case 2:
                                Map = Maps.Mapa_les;
                                playerPos = Maps.Mapa_les_Player_Pos;
                                break;
                            //3. MapaWalls
                            case 3:
                                Map = Maps.Mapa_walls;
                                playerPos = Maps.Mapa_walls_Player_Pos;
                                break;
                        }
                        break;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void MapDisplay() 
        {
            for(int v = 0; v<Map.GetLength(0); v++)
            {   for(int h = 0; h<Map.GetLength(1); h++)
                {   switch (Map.GetValue(v,h))
                    {
                        case 0: Console.Write(" "); break;
                        case 1: Console.Write("#"); break;
                        case 2: Console.Write("@"); break;
                        case 3: Console.Write("*"); break;
                        default: Console.Write("?"); break;
                    } 
                    Console.Write(" ");
                } 
                Console.Write("\n");
            }
        }
        public void VictoryScrean(int start = 0, bool reverse = false) 
        {
            /*
<=======================================================================>
   __   __   __     ______     ______   ______     ______     __  __    
  /\ \ / /  /\ \   /\  ___\   /\__  _\ /\  __ \   /\  == \   /\ \_\ \   
  \ \ \'/   \ \ \  \ \ \____  \/_/\ \/ \ \ \/\ \  \ \  __<   \ \____ \  
   \ \__|    \ \_\  \ \_____\    \ \_\  \ \_____\  \ \_\ \_\  \/\_____\ 
    \/_/      \/_/   \/_____/     \/_/   \/_____/   \/_/ /_/   \/_____/ 
                                                                        
<=======================================================================>

            */
            // makes "<=======================================================================>" with * in position
            string Line() {
                //45 znaku
                string ret = "<";
                for (int i = 0; i < start;i++) ret += "=";
                ret += "*";
                for (int i = start+1; i < 71;i++) ret += "=";
                return ret+">";
            }
            string ReverseLine(string line) {   
                string ret = "";
                for (int i = line.Length-2; i >= 1; i--)
                ret += line[i];
                return "<"+ret+">";
            }
            do {
            Console.Clear();
            if (Config.debug) Console.WriteLine($"{start} {reverse} \n");
            if (reverse) Console.WriteLine(ReverseLine(Line()));
            else Console.WriteLine(Line());
            Console.WriteLine(@"   __   __   __     ______     ______   ______     ______     __  __    
  /\ \ / /  /\ \   /\  ___\   /\__  _\ /\  __ \   /\  == \   /\ \_\ \   
  \ \ \'/   \ \ \  \ \ \____  \/_/\ \/ \ \ \/\ \  \ \  __<   \ \____ \  
   \ \__|    \ \_\  \ \_____\    \ \_\  \ \_____\  \ \_\ \_\  \/\_____\ 
    \/_/      \/_/   \/_____/     \/_/   \/_____/   \/_/ /_/   \/_____/ 
                                                                        ");
            if (reverse) Console.WriteLine(Line());
            else Console.WriteLine(ReverseLine(Line()));
            Thread.Sleep(100);
            if (start >= 70) VictoryScrean(0, !reverse);
            VictoryScrean(start+=1, reverse);
            } while (true);
        }
    }
}
