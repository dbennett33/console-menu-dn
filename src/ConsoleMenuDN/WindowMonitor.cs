namespace ConsoleMenuDN
{
    public class WindowMonitor
    {
        private int _windowWidth = Console.WindowWidth;
        private int _windowHeight = Console.WindowHeight;
        private readonly Action _onResize;
        private readonly Func<bool> _isInMenu;

        public WindowMonitor(Action onResize, Func<bool> isInMenu)
        {
            _onResize = onResize;
            _isInMenu = isInMenu;
        }

        public async Task MonitorWindowResizeAsync()
        {
            while (true)
            {
                if (_windowWidth != Console.WindowWidth || _windowHeight != Console.WindowHeight)
                {
                    _windowWidth = Console.WindowWidth;
                    _windowHeight = Console.WindowHeight;

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
