using ConsoleMenuDN.Interfaces;

namespace ConsoleMenuDN
{
    public class MenuState
    {
        public bool InMenu { get; set; } = true;
        public IConsoleWrapper ConsoleWrapper { get; set; } = new ConsoleWrapper();
    }
}
