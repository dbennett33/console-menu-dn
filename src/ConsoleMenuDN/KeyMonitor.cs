namespace ConsoleMenuDN
{
    internal class KeyMonitor
    {
        private readonly List<MenuOption> _menuOptions;
        private readonly Action<int> _updateSelectedItem;
        private readonly Func<int> _getSelectedItem;
        private readonly Action _returnToMenu;
        private readonly MenuState _menuState;

        internal KeyMonitor(List<MenuOption> menuOptions,
                          Action<int> updateSelectedItem,
                          Func<int> getSelectedItem,
                          Action returnToMenu,
                          MenuState menuState)
        {
            _menuOptions = menuOptions;
            _updateSelectedItem = updateSelectedItem;
            _getSelectedItem = getSelectedItem;
            _returnToMenu = returnToMenu;
            _menuState = menuState;
        }

        internal async Task MonitorKeyInputAsync()
        {
            while (true)
            {
                if (_menuState.InMenu)
                {
                    var key = Console.ReadKey(true).Key;
                    await HandleKeyInput(key);
                }

                await Task.Delay(100);
            }
        }

        private async Task HandleKeyInput(ConsoleKey key)
        {
            int selectedItem = _getSelectedItem();

            if (Keybinds.UpKeys.Contains(key))
            {
                selectedItem--;
                if (selectedItem < 0)
                {
                    selectedItem = _menuOptions.Count - 1;
                }
                _updateSelectedItem(selectedItem);
            }
            else if (Keybinds.DownKeys.Contains(key))
            {
                selectedItem++;
                if (selectedItem >= _menuOptions.Count)
                {
                    selectedItem = 0;
                }
                _updateSelectedItem(selectedItem);
            }
            else if (Keybinds.EnterKeys.Contains(key))
            {
                await SelectItem(selectedItem);
            }
        }

        private async Task SelectItem(int selectedItem)
        {
            Console.Clear();
            Console.CursorVisible = true;
            _menuState.InMenu = false;

            await _menuOptions[selectedItem].Action();

            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            _returnToMenu();
        }
    }
}
