namespace ConsoleMenuDN
{
    /// <summary>
    /// Represents a menu manager that handles the rendering and interaction of a console menu.
    /// </summary>
    public class MenuManager
    {
        private readonly List<MenuItem> _menuOptions;
        private readonly MenuRenderer _renderer;
        private readonly WindowMonitor _windowMonitor;
        private readonly KeyMonitor _keyMonitor;
        private readonly MenuState _menuState;
        private readonly MenuSettings _menuSettings;

        private int _selectedItem = 0;

        /// <summary>
        /// Initializes a new instance of the MenuManager class.
        /// </summary>
        /// <param name="menuOptions">The list of menu options.</param>
        /// <param name="title">The title of the menu.</param>
        public MenuManager(List<MenuItem> menuOptions, string title, MenuSettings? menuSettings = null)
        {
            if (menuSettings == null)
            {
                menuSettings = new MenuSettings();
            }

            _menuOptions = menuOptions;
            _menuState = new MenuState();
            _menuSettings = menuSettings;
            _renderer = new MenuRenderer(title, _menuOptions, _menuSettings);
            _windowMonitor = new WindowMonitor(RedrawMenu, () => _menuState.InMenu);
            _keyMonitor = new KeyMonitor(_menuOptions, UpdateSelectedItem, GetSelectedItem, ReturnToMenu, _menuState);
        }

        /// <summary>
        /// Shows the console menu.
        /// </summary>
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
