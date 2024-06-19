using ConsoleMenuDN.Interfaces;

namespace ConsoleMenuDN
{
    public class WindowMonitor : IWindowMonitor
    {
        public int _windowWidth;
        public int _windowHeight;
        public readonly Action _onResize;
        public readonly Func<bool> _isInMenu;
        private readonly MenuState _menuState;

        public WindowMonitor(Action onResize, Func<bool> isInMenu, MenuState menuState)
        {
            _onResize = onResize;
            _isInMenu = isInMenu;
            _menuState = menuState;

            _windowHeight = _menuState.ConsoleWrapper.BufferHeight;
            _windowWidth = _menuState.ConsoleWrapper.BufferWidth;
        }

        public async Task MonitorWindowResizeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_isInMenu())
                {
                    if (_windowWidth != _menuState.ConsoleWrapper.BufferWidth || _windowHeight != _menuState.ConsoleWrapper.BufferHeight)
                    {
                        _windowWidth = _menuState.ConsoleWrapper.BufferWidth;
                        _windowHeight = _menuState.ConsoleWrapper.BufferHeight;

                        _onResize();
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}
