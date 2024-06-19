namespace ConsoleMenuDN
{
    public class MenuItem
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public Func<Task> Action { get; set; }  // Change Action to Func<Task> for async support
        internal int YStartPos { get; set; }
        internal int XStartPos { get; set; }

        // Constructor for asynchronous actions
        public MenuItem(string name, Func<Task> action)
        {
            Name = name;
            Action = action;
        }

        // Constructor for synchronous actions
        public MenuItem(string name, Action action)
        {
            Name = name;
            Action = () => Task.Run(action);
        }
    }
}
