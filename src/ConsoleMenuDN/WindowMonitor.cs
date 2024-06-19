namespace ConsoleMenuDN
{
    internal class WindowMonitor
    {
        private int _windowWidth = Console.BufferWidth;
        private int _windowHeight = Console.BufferHeight;
        private readonly Action _onResize;
        private readonly Func<bool> _isInMenu;

        internal WindowMonitor(Action onResize, Func<bool> isInMenu)
        {
            _onResize = onResize;
            _isInMenu = isInMenu;
        }

        internal async Task MonitorWindowResizeAsync()
        {
            while (true)
            {
                if (_windowWidth != Console.BufferWidth || _windowHeight != Console.BufferHeight)
                {
                    _windowWidth = Console.BufferWidth;
                    _windowHeight = Console.BufferHeight;

                    if (_isInMenu())
                    {
                        _onResize();
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}
