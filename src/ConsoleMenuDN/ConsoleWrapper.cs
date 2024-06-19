using ConsoleMenuDN.Interfaces;

namespace ConsoleMenuDN
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public int WindowWidth => Console.WindowWidth;
        public int WindowHeight => Console.WindowHeight;
        public int BufferWidth => Console.BufferWidth;
        public int BufferHeight => Console.BufferHeight;

        public void Clear() => Console.Clear();
        public void SetCursorVisible(bool visible) => Console.CursorVisible = visible;
        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
        public void WriteLine(string value) => Console.WriteLine(value);
        public void Write(string value) => Console.Write(value);
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);     
    }
}
