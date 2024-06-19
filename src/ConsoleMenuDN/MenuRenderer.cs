namespace ConsoleMenuDN
{
    internal class MenuRenderer
    {
        private readonly string _title;
        private readonly List<MenuItem> _menuOptions;
        private readonly MenuSettings _menuSettings;

        private int _centreX;

        internal MenuRenderer(string title, List<MenuItem> menuOptions, MenuSettings menuSettings)
        {
            _title = title;
            _menuOptions = menuOptions;
            _menuSettings = menuSettings;
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
            // TODO: This all assumes the header text is only 1 line high
            // - users might want fancy ASCII art headers and stuff so need to calculate height

            int currentRow = 4;
            int offset = 0;

            if (_menuSettings.Indentation == MenuSettings.IdentationType.Left)
            {
                offset = _menuOptions.OrderByDescending(mo => mo.Name.Length).First().Name.Length;
            }            

            foreach (var mo in _menuOptions)
            {
                mo.YStartPos = currentRow;
                    
                if (offset > 0)
                {
                    mo.XStartPos = _centreX - (offset / 2);
                }
                else
                {
                    mo.XStartPos = _centreX - (mo.Name.Length / 2);
                }  

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
