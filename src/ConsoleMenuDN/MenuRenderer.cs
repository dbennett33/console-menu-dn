namespace ConsoleMenuDN
{
    internal class MenuRenderer
    {
        private readonly string _title;
        private readonly List<MenuOption> _menuOptions;
        private int _centreX;

        internal MenuRenderer(string title, List<MenuOption> menuOptions)
        {
            _title = title;
            _menuOptions = menuOptions;
        }

        internal void RedrawMenu(int selectedItem)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            _centreX = Console.BufferWidth / 2;
            DrawHeader();
            DrawMenu(selectedItem);
            RefreshMenu(selectedItem);
        }

        internal void RefreshMenu(int selectedItem)
        {
            foreach (var mo in _menuOptions)
            {
                if (_menuOptions.IndexOf(mo) == selectedItem)
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

        private void DrawMenu(int selectedItem)
        {
            int currentRow = 4;
            
            foreach (var mo in _menuOptions)
            {
                mo.YStartPos = currentRow;
                mo.XStartPos = _centreX - (mo.Name.Length / 2);

                Draw(mo.Name, mo.XStartPos, currentRow);

                currentRow++;
            }

            Console.SetCursorPosition(_menuOptions[0].XStartPos, _menuOptions[0].YStartPos);
        }

        private void DrawHeader()
        {
            // TODO: This all assumes the header text is only 1 line high
            // - users might want fancy ASCII art headers and stuff so need to calculate height

            // 50% header bar

            //for (int i = _centreX / 2; i < (_centreX / 2) * 3; i++)
            //{
            //    Draw("=", i, 0);
            //    Draw("=", i, 2);
            //}

            // 100% header bar

            for (int i = 0; i < Console.BufferWidth; i++)
            {
                Draw("=", i, 0);
                Draw("=", i, 2);
            }

            Draw(_title, _centreX - (_title.Length / 2), 1);
        }

        private void Draw(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException)
            {
                // window size is now smaller than the menu
            }
        }
    }
}
