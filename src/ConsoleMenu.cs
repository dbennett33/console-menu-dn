namespace ConsoleMenuDN
{
    public class ConsoleMenu
    {
        private readonly List<MenuOption> _menuOptions;

        private int _centreX = Console.BufferWidth / 2;
        private int _selectedItem = 0;

        public ConsoleMenu(List<MenuOption> menuOptions)
        {
            _menuOptions = menuOptions;
        }

        public void Show()
        {
            Console.CursorVisible = false;
            DrawHeader();
            DrawMenu();
            Run();
        }

        private void Run()
        { 
            bool inMenu = true;
            while (inMenu)
            {
                RefreshMenu();
                
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    _selectedItem--;
                    if (_selectedItem < 0)
                    {
                        _selectedItem = _menuOptions.Count - 1;
                    }
                }
                if (key == ConsoleKey.DownArrow)
                {
                    _selectedItem++;
                    if (_selectedItem > _menuOptions.Count - 1)
                    {
                        _selectedItem = 0;
                    }
                }
                if (key == ConsoleKey.Enter)
                {
                    Console.Clear();    
                    _menuOptions[_selectedItem].Action();

                    // TODO: return to menu option
                    if (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Show();
                    }
                    else
                    {
                       inMenu = false;
                    }
                }
            }
        }

        private void RefreshMenu()
        {
            foreach (var mo in _menuOptions)
            {
                if (_menuOptions.IndexOf(mo) == _selectedItem)
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


        private void DrawMenu()
        {
            int currentRow = 4;

            foreach (var mo in _menuOptions)
            {
                mo.YStartPos = currentRow;
                mo.XStartPos = _centreX - mo.Name.Length / 2;

                Draw(mo.Name, mo.XStartPos, currentRow);

                currentRow++;
            }

            Console.SetCursorPosition(_menuOptions[0].XStartPos, _menuOptions[0].YStartPos);
        }

        private void DrawHeader()
        {
            for (int i = _centreX / 2; i < (_centreX / 2) * 3; i++)
            {
                Draw("=", i, 0);
                Draw("=", i, 2);
            }

            Draw("Menu", _centreX - 2, 1);
        }

        private void Draw(string s, int x, int y)
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
