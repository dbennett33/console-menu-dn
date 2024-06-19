namespace ConsoleMenuDN.Interfaces
{
    public interface IConsoleWrapper
    {
        int WindowWidth { get; }
        int WindowHeight { get; }
        int BufferWidth { get; }
        int BufferHeight { get; }
        void Clear();
        void SetCursorVisible(bool visible);
        void SetCursorPosition(int left, int top);
        void WriteLine(string value);
        void Write(string value);
        ConsoleKeyInfo ReadKey(bool intercept);
    }
}
