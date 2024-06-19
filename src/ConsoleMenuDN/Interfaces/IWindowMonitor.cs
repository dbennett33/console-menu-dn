namespace ConsoleMenuDN.Interfaces
{
    public interface IWindowMonitor
    {
        Task MonitorWindowResizeAsync(CancellationToken cancellationToken);
    }
}