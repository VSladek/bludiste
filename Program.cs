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
            maze.MapAction(3, menu.mapSelect);
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
        public int selectedMainPosition, selectedMapsPosition, selectedOptionsPosition, mapSelect=1;
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
            if (selectedMapsPosition==0) selectedMapsPosition=mapSelect;
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
                        case 1: selectedMapsPosition = 1; mapSelect = 1; break;
                        case 2: selectedMapsPosition = 2; mapSelect = 2; break;
                        case 3: selectedMapsPosition = 3; mapSelect = 3; break;
                        case 4: selectedMapsPosition = 4; mapSelect = 4; break;
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
                        case 2: if (Config.mapSize != 10) Config.mapSize--; break;
                        case 3: Config.useAlternativeDisplay = !Config.useAlternativeDisplay; break;
                        case 4: Config.useSpaceInBetweenWalls = !Config.useSpaceInBetweenWalls; break;
                        case 5: Config.randomGeneration = !Config.randomGeneration; break;
                        case 6: if (Config.randomGenerationFillPercentage<100) Config.randomGenerationFillPercentage+=5;
                                else Config.randomGenerationFillPercentage=5; break;
                        case 7: Config.mapSizeRounderSystem = !Config.mapSizeRounderSystem; break;
                        case 8: Config.showGeneration = !Config.showGeneration; break;
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
            Console.WriteLine($"Maps:");
            if (selectedMapsPosition == 1) Console.Write("> "); Console.WriteLine("Generated");
            if (selectedMapsPosition == 2) Console.Write("> "); Console.WriteLine("Mapa Les");
            if (selectedMapsPosition == 3) Console.Write("> "); Console.WriteLine("Mapa Walls");
            if (selectedMapsPosition == 4) Console.Write("> "); Console.WriteLine("Mapa Cave");
        }
        private void OptionOptions() {
            Console.WriteLine($"Options:");
            if (selectedOptionsPosition == 1) Console.Write("> "); Console.WriteLine($"Bigger generated map size ({Config.mapSize})");
            if (selectedOptionsPosition == 2) Console.Write("> "); Console.WriteLine($"Smaller generated map size ({Config.mapSize})");
            if (selectedOptionsPosition == 3) Console.Write("> "); Console.WriteLine($"Use alternative display ({Config.useAlternativeDisplay})");
            if (selectedOptionsPosition == 4) Console.Write("> "); Console.WriteLine($"Use space in between walls ({Config.useSpaceInBetweenWalls})");
            if (selectedOptionsPosition == 5) Console.Write("> "); Console.WriteLine($"Turns on random generation without algorithm ({Config.randomGeneration})");
            if (selectedOptionsPosition == 6) Console.Write("> "); Console.WriteLine($"Random generation fill percentage ({Config.randomGenerationFillPercentage})");
            if (selectedOptionsPosition == 7) Console.Write("> "); Console.WriteLine($"The algorithm likes odd numbers round map size to make him happy ({Config.mapSizeRounderSystem})");
            if (selectedOptionsPosition == 8) Console.Write("> "); Console.WriteLine($"Show algorithm generation ({Config.showGeneration})");
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
            void Down() { if (selectedMapsPosition!=4) selectedMapsPosition++; }
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
            void Down() { if (selectedOptionsPosition!=8) selectedOptionsPosition++; }
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
        public static int mapSize = 50;
        public static bool useAlternativeDisplay = true;
        public static bool useSpaceInBetweenWalls = false;
        public static bool randomGeneration = false;
        public static short randomGenerationFillPercentage = 35;
        public static bool mapSizeRounderSystem = true;
        public static bool showGeneration = true;
        public static bool debug = false;
    }
    public class Generated {
        internal class Cell { internal bool visited, blocked; internal int row, column; }
        private static Random ran = new Random();
        public int[,] Generated_Map;
        private Cell[,] cells;
        public int[] GeneratedplayerPos;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Generated() {
            if (Config.mapSizeRounderSystem) RounderSystem();
            Generated_Map = new int[Config.mapSize, Config.mapSize];
            GeneratedplayerPos = new int[2];
            if (Config.randomGeneration) GenerateRandom();
            else {
                cells = new Cell[Config.mapSize, Config.mapSize];
                // Initialize cell array.
                for (int row = 0; row < Config.mapSize; row++)
                for (int colum = 0; colum < Config.mapSize; colum++)
                cells[row, colum] = new Cell {   
                    visited = false, 
                    blocked = true,
                    row = row, 
                    column = colum };
                Generate();
            }
            GeneratePlayerPos:
            int[] generatePlayerPos = { 
                ran.Next(0, Config.mapSize-1), 
                ran.Next(0, Config.mapSize-1) 
                };
            if (Generated_Map[generatePlayerPos[0], generatePlayerPos[1]] != 1) {
                Generated_Map[generatePlayerPos[0], generatePlayerPos[1]] = 2;
                GeneratedplayerPos = generatePlayerPos;
            }
            else goto GeneratePlayerPos;
            GenerateGoalPos:
            int[] generateGoalPos = { 
                ran.Next(0, Config.mapSize-1), 
                ran.Next(0, Config.mapSize-1) 
                };
            if (Generated_Map[generateGoalPos[0], generateGoalPos[1]] != 1 || Generated_Map[generatePlayerPos[0], generatePlayerPos[1]] != 2)
            Generated_Map[generateGoalPos[0], generateGoalPos[1]] = 3;
            else goto GenerateGoalPos;
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private void RounderSystem()
        {
            if (Config.mapSize %2 == 0) Config.mapSize++;
        }

        private List<Cell> GetNeighbourCells(Cell cell)
        {
            List<Cell> neighbourCells = new List<Cell>();

            int row = cell.row;
            int col = cell.column;

            if (row > 2 && !cells[row - 2, col].visited)
                neighbourCells.Add(cells[row - 2, col]);
            if (col > 2 && !cells[row, col - 2].visited)
                neighbourCells.Add(cells[row, col - 2]);
            if (row < Config.mapSize - 3 && !cells[row + 2, col].visited)
                neighbourCells.Add(cells[row + 2, col]);
            if (col < Config.mapSize - 3 && !cells[row, col + 2].visited)
                neighbourCells.Add(cells[row, col + 2]);

            return neighbourCells;
        }
        internal void GenerateRandom() 
        {
            for (int size = 0; size < Config.mapSize; size++){
            Generated_Map[0,size] = 1;
            Generated_Map[size,0] = 1;
            Generated_Map[Config.mapSize-1,size] = 1;
            Generated_Map[size,Config.mapSize-1] = 1;
            } 
            if (Config.debug) Console.WriteLine($"{Config.mapSize} {4*Config.mapSize-4} {Config.mapSize * Config.mapSize-(4*Config.mapSize-4)} {(float)Config.randomGenerationFillPercentage/100} {(int)((Config.mapSize * Config.mapSize-(4*Config.mapSize-4)) * ((float)Config.randomGenerationFillPercentage/100))}");
            for (int fill = 0; fill <= (int)((Config.mapSize * Config.mapSize-(4*Config.mapSize-4)) * ((float)Config.randomGenerationFillPercentage/100)); fill++) {
                Generation:
                int[] generateWall = { 
                    ran.Next(0, Config.mapSize-1), 
                    ran.Next(0, Config.mapSize-1) 
                    };
                if (Generated_Map[generateWall[0], generateWall[1]] != 1) 
                Generated_Map[generateWall[0], generateWall[1]] = 1;
                else goto Generation;
            }
        }
        public void Generate()
        {
            var stack = new Stack<Cell>();
            int counter = 5;
            // Add starting cell to top of stack, mark visited.
            stack.Push(cells[1, 1]);
            stack.Peek().visited = true;
            stack.Peek().blocked = false;
            
            while (stack.Any()) {
                // Get current cell from top of stack.
                Cell thisCell = stack.Peek();
                if (Config.debug ||(Config.showGeneration && counter>5)){
                    if (Config.showGeneration) Console.Clear();
                    for (int row = 0; row < Config.mapSize; row++) {
                    for (int colum = 0; colum < Config.mapSize; colum++)
                        Console.Write(cells[row, colum].blocked ? "██" : "  ");
                        Console.WriteLine();
                    }
                    if (Config.showGeneration) { counter=0; Thread.Sleep(35); }
                } else counter++;
                // Get unvisited neighbour cells.
                List<Cell> neighbourCells = GetNeighbourCells(thisCell);

                if (neighbourCells.Any()) {
                    // Randomly select neighbour cell.
                    Cell nextCell = neighbourCells[ran.Next(neighbourCells.Count)];

                    // Get wall cell between current and neighbour cells.
                    int midRow = thisCell.row + (nextCell.row - thisCell.row) / 2;
                    int midColum = thisCell.column + (nextCell.column - thisCell.column) / 2;
                    Cell wallCell = cells[midRow, midColum];

                    // Mark neighbour and wall cells as unblocked.
                    nextCell.blocked = false;
                    wallCell.blocked = false;

                    // Add neighbour cell to top of stack, mark visited.
                    stack.Push(nextCell);
                    nextCell.visited = true;
                }
                else stack.Pop();
            }
            for (int row = 0; row < Config.mapSize; row++)
            for (int colum = 0; colum < Config.mapSize; colum++)
            Generated_Map[row,colum] = cells[row, colum].blocked ? 1 : 0;
        }
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
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            { 1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
            { 1, 0, 1, 0, 0, 0, 1, 0, 0, 1},
            { 1, 0, 1, 0, 0, 1, 1, 0, 0, 1},
            { 1, 2, 1, 1, 1, 1, 0, 0, 0, 1},
            { 1, 0, 1, 1, 0, 0, 0, 1, 0, 1},
            { 1, 0, 1, 0, 0, 1, 0, 1, 0, 1},
            { 1, 0, 1, 0, 1, 0, 0, 1, 0, 1},
            { 1, 0, 0, 0, 1, 0, 0, 1, 3, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        public static int[] Mapa_les_Player_Pos = { 5, 2 };
        public static int[,] Mapa_walls =
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 1, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 2, 1, 0, 3, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        public static int[] Mapa_walls_Player_Pos = { 1, 1 };
        public static int[,] Mapa_cave =
        {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1},
            {1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1},
            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1},
            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1},
            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1},
            {1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
            {1, 0, 1, 3, 1, 0, 1, 3, 1, 0, 0, 1},
            {1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        public static int[] Mapa_cave_Player_Pos = {1, 7};
    }
    class Maze
    {
        public int[,] Map;
        public int[] playerPos;
        public Maze() {
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
        public int moveCounter;
        public void MapAction(int action, int value = 0)
        {
            try {
            switch (action)
                {
                    //1. display
                    case 1:
                        MapDisplay();
                        break;
                    //2. player move
                    case 2:
                        moveCounter++;
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
                                else throw new Exception("Invalid move"+" "+playerPos[0]+" "+playerPos[1]);
                                break;
                            case 2:
                                if ((int?)Map.GetValue(playerPos[0] + 1, playerPos[1]) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[0]++; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0] + 1, playerPos[1]) == 3) VictoryScrean();
                                else throw new Exception("Invalid move"+" "+playerPos[0]+" "+playerPos[1]);
                                break;
                            case 3:
                                if ((int?)Map.GetValue(playerPos[0], playerPos[1] + 1) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[1]++; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0], playerPos[1] + 1) == 3) VictoryScrean();
                                else throw new Exception("Invalid move"+" "+playerPos[0]+" "+playerPos[1]);
                                break;
                            case 4:
                                if ((int?)Map.GetValue(playerPos[0], playerPos[1] - 1) == 0) { Map[playerPos[0], playerPos[1]] = 0; playerPos[1]--; Map[playerPos[0], playerPos[1]] = 2; }
                                else if ((int?)Map.GetValue(playerPos[0], playerPos[1] - 1) == 3) VictoryScrean();
                                else throw new Exception("Invalid move"+" "+playerPos[0]+" "+playerPos[1]);
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
                                Generated mapGen = new Generated();
                                Map = mapGen.Generated_Map;
                                playerPos = mapGen.GeneratedplayerPos;
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
                            //4. Mapa Cave
                            case 4:
                                Map = Maps.Mapa_cave;
                                playerPos = Maps.Mapa_cave_Player_Pos;
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
                        case 0: Console.Write(Config.useAlternativeDisplay ? "  " : " "); break;
                        case 1: Console.Write(Config.useAlternativeDisplay ? "██" : "#"); break;
                        case 2: Console.Write(Config.useAlternativeDisplay ? ":p" : "@"); break;
                        case 3: Console.Write(Config.useAlternativeDisplay ? "|▀" : "*"); break;
                        default: Console.Write("?"); break;
                    } 
                    if (Config.useSpaceInBetweenWalls) Console.Write(" ");
                } 
                Console.Write("\n");
            }
        }
        public void VictoryScrean(int start = 0, bool reverse = false) 
        {
            string Line() {
                //71 znaku
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
            Console.WriteLine($"\nYou achieved the goal in {moveCounter} moves!");
            Thread.Sleep(100);
            if (start >= 70) VictoryScrean(0, !reverse);
            VictoryScrean(start+=1, reverse);
            } while (true);
        }
    }
}
