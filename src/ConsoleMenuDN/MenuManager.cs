namespace ConsoleMenuDN
{
    public class MenuManager
    {
        private readonly List<MenuOption> _menuOptions;
        private readonly MenuRenderer _renderer;
        private readonly WindowMonitor _windowMonitor;
        private readonly KeyMonitor _keyMonitor;
        private readonly MenuState _menuState;

        private int _selectedItem = 0;

        public MenuManager(List<MenuOption> menuOptions, string title)
        {
            _menuOptions = menuOptions;
            _menuState = new MenuState();
            _renderer = new MenuRenderer(title, _menuOptions);
            _windowMonitor = new WindowMonitor(RedrawMenu, () => _menuState.InMenu);
            _keyMonitor = new KeyMonitor(_menuOptions, UpdateSelectedItem, GetSelectedItem, ReturnToMenu, _menuState);
        }

        public void Show()
        {
            Console.Clear();
            Console.CursorVisible = false;

            _renderer.RedrawMenu(_selectedItem);
            Run().GetAwaiter().GetResult();
        }

        private void RedrawMenu()
        {
            if (_menuState.InMenu)
            {
                _renderer.RedrawMenu(_selectedItem);
            }
        }

        private async Task Run()
        {
            var windowResizeTask = _windowMonitor.MonitorWindowResizeAsync();
            var keyInputTask = _keyMonitor.MonitorKeyInputAsync();

            await Task.WhenAll(windowResizeTask, keyInputTask);
        }

        private void UpdateSelectedItem(int selectedItem)
        {
            _selectedItem = selectedItem;
            _renderer.RefreshMenu(_selectedItem);
        }

        private int GetSelectedItem()
        {
            return _selectedItem;
        }

        private void ReturnToMenu()
        {
            _menuState.InMenu = true;
            Console.Clear();
            Console.CursorVisible = false;
            _renderer.RedrawMenu(_selectedItem);
        }
    }
}
