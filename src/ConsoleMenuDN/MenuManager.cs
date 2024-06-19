using ConsoleMenuDN.Interfaces;

namespace ConsoleMenuDN
{
    /// <summary>
    /// Represents a menu manager that handles the rendering and interaction of a console menu.
    /// </summary>
    public class MenuManager
    {
        public readonly List<MenuItem> _menuOptions;
        public readonly MenuRenderer _renderer;
        public readonly IWindowMonitor _windowMonitor;
        public readonly IKeyMonitor _keyMonitor;
        public readonly MenuState _menuState;
        public readonly MenuSettings _menuSettings;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
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
            _renderer = new MenuRenderer(title, _menuOptions, _menuSettings, _menuState);
            _windowMonitor = new WindowMonitor(RedrawMenu, () => _menuState.InMenu, _menuState);
            _keyMonitor = new KeyMonitor(_menuOptions, UpdateSelectedItem, GetSelectedItem, ReturnToMenu, _menuState);
        }

        public MenuManager(List<MenuItem> menuOptions,
                           string title,
                           MenuRenderer renderer,
                           IWindowMonitor windowMonitor,
                           IKeyMonitor keyMonitor,
                           MenuState menuState,
                           MenuSettings? menuSettings = null)
        {
            if (menuSettings == null)
            {
                menuSettings = new MenuSettings();
            }

            _menuOptions = menuOptions;
            _menuState = menuState;
            _menuSettings = menuSettings;
            _renderer = renderer;
            _windowMonitor = windowMonitor;
            _keyMonitor = keyMonitor;
        }

        /// <summary>
        /// Shows the console menu.
        /// </summary>
        public void Show()
        {
            _menuState.ConsoleWrapper.Clear();
            _menuState.ConsoleWrapper.SetCursorVisible(false);

            _renderer.RedrawMenu(_selectedItem);
            Run().GetAwaiter().GetResult();
        }

        public void RedrawMenu()
        {
            if (_menuState.InMenu)
            {
                _renderer.RedrawMenu(_selectedItem);
            }
        }

        public async Task Run()
        {
            var windowResizeTask = _windowMonitor.MonitorWindowResizeAsync(_cancellationTokenSource.Token);
            var keyInputTask = _keyMonitor.MonitorKeyInputAsync(_cancellationTokenSource.Token);

            await Task.WhenAll(windowResizeTask, keyInputTask);
        }

        public void UpdateSelectedItem(int selectedItem)
        {
            _selectedItem = selectedItem;
            _renderer.RefreshMenu(_selectedItem);
        }

        public int GetSelectedItem()
        {
            return _selectedItem;
        }

        public void ReturnToMenu()
        {
            _menuState.InMenu = true;
            _menuState.ConsoleWrapper.Clear();
            _menuState.ConsoleWrapper.SetCursorVisible(false);
            _renderer.RedrawMenu(_selectedItem);
        }
        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
