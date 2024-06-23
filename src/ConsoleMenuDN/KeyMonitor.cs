using ConsoleMenuDN.Interfaces;

namespace ConsoleMenuDN
{
    public class KeyMonitor : IKeyMonitor
    {
        public readonly List<MenuItem> _menuOptions;
        public readonly Action<int> _updateSelectedItem;
        public readonly Func<int> _getSelectedItem;
        public readonly Action _returnToMenu;
        public readonly MenuState _menuState;

        public KeyMonitor(List<MenuItem> menuOptions,
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

        public async Task MonitorKeyInputAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_menuState.InMenu)
                {
                    var key = _menuState.ConsoleWrapper.ReadKey(true).Key;
                    await HandleKeyInput(key);
                }

                await Task.Delay(100);
            }
        }

        public async Task HandleKeyInput(ConsoleKey key)
        {
            int selectedItem = _getSelectedItem();

            if (Keybinds.UpKeys.Contains(key))
            {
                MoveUp(selectedItem);
            }
            else if (Keybinds.DownKeys.Contains(key))
            {
                MoveDown(selectedItem);
            }
            else if (Keybinds.EnterKeys.Contains(key))
            {
                await SelectItem(selectedItem);
            }
            else if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
            {
                await SelectItemViaNumberKey(key);
            }
        }

        private async Task SelectItemViaNumberKey(ConsoleKey key)
        {
            int keyNumber = (int)key - (int)ConsoleKey.D0;
            keyNumber--;
            if (keyNumber < _menuOptions.Count)
            {
                await SelectItem(keyNumber);
            }
        }

        private int MoveDown(int selectedItem)
        {
            selectedItem++;
            if (selectedItem >= _menuOptions.Count)
            {
                selectedItem = 0;
            }
            _updateSelectedItem(selectedItem);
            return selectedItem;
        }

        private int MoveUp(int selectedItem)
        {
            selectedItem--;
            if (selectedItem < 0)
            {
                selectedItem = _menuOptions.Count - 1;
            }
            _updateSelectedItem(selectedItem);
            return selectedItem;
        }

        public async Task SelectItem(int selectedItem)
        {
            _menuState.ConsoleWrapper.Clear();
            _menuState.ConsoleWrapper.SetCursorVisible(true);
            _menuState.InMenu = false;

            await _menuOptions[selectedItem].Action();

            _menuState.ConsoleWrapper.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            _returnToMenu();
        }
    }
}
