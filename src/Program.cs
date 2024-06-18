namespace ConsoleMenuDN
{
    internal class Program
    {

        static void Main(string[] args)
        {
            int selectedItem = 0;

            var menuOptions = new List<MenuOption>()
            {
                new MenuOption() { Name = "Option 1", Action = () => { Console.WriteLine("Option 1 Selected"); }},
                new MenuOption() { Name = "Option 2", Action = () => { Console.WriteLine("Option 2 Selected"); }},
                new MenuOption() { Name = "Option 3", Action = () => { Console.WriteLine("Option 3 Selected"); }},
                new MenuOption() { Name = "Option 4", Action = () => { Console.WriteLine("Option 4 Selected"); }},
                new MenuOption() { Name = "Quit", Action = () => { Environment.Exit(0); }}
            };
            Console.ResetColor();
            var centreX = Console.BufferWidth / 2;

            DrawHeader(centreX);

            int currentRow = 4;

            foreach (var mo in menuOptions)
            {
                mo.YStartPos = currentRow;
                mo.XStartPos = centreX - mo.Name.Length / 2;

                Draw(mo.Name, mo.XStartPos, currentRow);

                currentRow++;
            }    

            Console.SetCursorPosition(menuOptions[0].XStartPos, menuOptions[0].YStartPos);

            Console.CursorVisible = false;
            bool inMenu = true;

            while (inMenu)
            {
                DrawMenu(menuOptions, selectedItem);

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedItem--;
                    if (selectedItem < 0)
                    {
                        selectedItem = menuOptions.Count - 1;
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedItem++;
                    if (selectedItem > menuOptions.Count - 1)
                    {
                        selectedItem = 0;
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    inMenu = false;
                    menuOptions[selectedItem].Action();

                }
            }


        }

        private static void DrawMenu(List<MenuOption> menuOptions, int selectedItem)
        {
            foreach (var mo in menuOptions)
            {
                if (menuOptions.IndexOf(mo) == selectedItem)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }

                Draw(mo.Name, mo.XStartPos, mo.YStartPos);

            }

            Console.ResetColor();
        }

        private static void DrawHeader(int centreX)
        {
            for (int i = centreX / 2; i < (centreX / 2) * 3; i++)
            {
                Draw("=", i, 0);
                Draw("=", i, 2);
            }

            Draw("Menu", centreX - 2, 1);
        }

        public class MenuOption
        {
            public string Name { get; set; }
            public Action Action { get; set; }
            public int YStartPos { get; set; }
            public int XStartPos { get; set; }

        }

        protected static void Draw(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}
