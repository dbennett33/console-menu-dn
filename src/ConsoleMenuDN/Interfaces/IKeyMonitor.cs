namespace ConsoleMenuDN.Interfaces
{
    public interface IKeyMonitor
    {
        Task HandleKeyInput(ConsoleKey key);
        Task MonitorKeyInputAsync(CancellationToken cancellationToken);
        Task SelectItem(int selectedItem);
    }
}