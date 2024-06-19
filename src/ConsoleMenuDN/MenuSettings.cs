namespace ConsoleMenuDN
{
    public class MenuSettings
    {
        public bool ShowLineNumbers { get; set; }
        public IdentationType Indentation { get; set; }

        public MenuSettings(bool showLineNumbers = true, IdentationType identation = IdentationType.Left)
        {
            ShowLineNumbers = showLineNumbers;
            Indentation = identation;
        }


        public enum IdentationType
        {
            Left,
            Center
        }

    }
}
