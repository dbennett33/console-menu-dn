using ConsoleMenuDN.Interfaces;

namespace ConsoleMenuDN
{
    public class MenuRenderer
    {
        public readonly string _title;
        public readonly List<MenuItem> _menuOptions;
        public readonly MenuSettings _menuSettings;
        public readonly MenuState _menuState;

        public int _centreX;

        public MenuRenderer(string title, List<MenuItem> menuOptions, MenuSettings menuSettings, MenuState menuState)
        {
            _title = title;
            _menuOptions = menuOptions;
            _menuSettings = menuSettings;
            _menuState = menuState;
        }

        public void RedrawMenu(int selectedItem)
        {
            _menuState.ConsoleWrapper.Clear();
            _menuState.ConsoleWrapper.WriteLine("\x1b[3J");
            _centreX = _menuState.ConsoleWrapper.BufferWidth / 2;
            DrawHeader();
            DrawMenu(selectedItem);
            RefreshMenu(selectedItem);
        }

        public void RefreshMenu(int selectedItem)
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

                Draw(GetDisplayName(mo), mo.XStartPos, mo.YStartPos);
            }
            Console.ResetColor();
        }

        public void DrawMenu(int selectedItem)
        {
            // TODO: This all assumes the header text is only 1 line high
            // - users might want fancy ASCII art headers and stuff so need to calculate height

            int currentRow = 4;
            int offset = 0;

            if (_menuSettings.Indentation == MenuSettings.IdentationType.Left)
            {
                offset = _menuOptions.OrderByDescending(mo => mo.Name.Length).First().Name.Length;

                if (_menuSettings.ShowLineNumbers)
                {
                    offset += 4;
                }
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

                Draw(GetDisplayName(mo), mo.XStartPos, currentRow);

                currentRow++;
            }

            _menuState.ConsoleWrapper.SetCursorPosition(_menuOptions[0].XStartPos, _menuOptions[0].YStartPos);
        }

        public string GetDisplayName(MenuItem mo)
        {
            string name;

            if (_menuSettings.ShowLineNumbers)
            {
                name = $"({_menuOptions.IndexOf(mo) + 1}) {mo.Name}";
            }
            else
            {
                name = mo.Name;
            }

            return name;
        }

        public void DrawHeader()
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

            for (int i = 0; i < _menuState.ConsoleWrapper.BufferWidth; i++)
            {
                Draw("=", i, 0);
                Draw("=", i, 2);
            }

            Draw(_title, _centreX - (_title.Length / 2), 1);
        }

        public void Draw(string s, int x, int y)
        {
            try
            {
                _menuState.ConsoleWrapper.SetCursorPosition(x, y);
                _menuState.ConsoleWrapper.Write(s);
            }
            catch (ArgumentOutOfRangeException)
            {
                // window size is now smaller than the menu
            }
        }
    }
}
