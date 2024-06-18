using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleMenuDN
{
    public class ConsoleMenu
    {
        private readonly List<MenuOption> _menuOptions;
        private readonly string _title;

        private int _centreX;
        private int _selectedItem = 0;

        private int _windowWidth = Console.WindowWidth;
        private int _windowHeight = Console.WindowHeight;

        private bool _inMenu = true;



        public ConsoleMenu(List<MenuOption> menuOptions, string title)
        {
            _menuOptions = menuOptions;
            _title = title;
        }

        public void Show()
        {
            Console.Clear();
            Console.CursorVisible = false;
            _centreX = Console.BufferWidth / 2;

            RedrawMenu();
            Run().GetAwaiter().GetResult();
        }

        private void RedrawMenu()
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J"); // magic
            DrawHeader();
            DrawMenu();
            RefreshMenu();
        }

        private async Task Run()
        {
            var windowResizeTask = MonitorWindowResizeAsync();

            while (_inMenu)
            {
                RefreshMenu();
                var key = Console.ReadKey(true).Key;
                await HandleKeyInput(key);
            }
        }

        private async Task HandleKeyInput(ConsoleKey key)
        {
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
                if (_selectedItem >= _menuOptions.Count)
                {
                    _selectedItem = 0;
                }
            }
            if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                Console.CursorVisible = true;
                _inMenu = false;
                await _menuOptions[_selectedItem].Action();
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                _inMenu = true;

                Console.Clear();
                Console.CursorVisible = false;
                DrawHeader();
                DrawMenu();
            }
        }

        private async Task MonitorWindowResizeAsync()
        {
            while (true)
            {
                if (_windowWidth != Console.WindowWidth || _windowHeight != Console.WindowHeight)
                {
                    _windowWidth = Console.WindowWidth;
                    _windowHeight = Console.WindowHeight;
                    _centreX = Console.BufferWidth / 2;

                    if (_inMenu)
                        RedrawMenu();
                }

                await Task.Delay(100); // Polling interval
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

            Draw(_title, _centreX - (_title.Length/2), 1);
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

        private void ClearConsole()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; y++)
            {
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
