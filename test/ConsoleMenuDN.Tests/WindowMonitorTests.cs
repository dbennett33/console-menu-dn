using ConsoleMenuDN;
using ConsoleMenuDN.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleMenuDN.Tests
{
    [TestClass]
    public class WindowMonitorTests
    {
        private IConsoleWrapper _consoleWrapper;
        private Action _onResize;
        private Func<bool> _isInMenu;
        private MenuState _menuState;
        private WindowMonitor _windowMonitor;
        private CancellationTokenSource _cancellationTokenSource;

        [TestInitialize]
        public void Setup()
        {
            _consoleWrapper = Substitute.For<IConsoleWrapper>();
            _consoleWrapper.BufferWidth.Returns(80);
            _consoleWrapper.BufferHeight.Returns(25);

            _menuState = new MenuState { ConsoleWrapper = _consoleWrapper };
            _onResize = Substitute.For<Action>();
            _isInMenu = Substitute.For<Func<bool>>();
            _isInMenu.Invoke().Returns(true);

            _windowMonitor = new WindowMonitor(_onResize, _isInMenu, _menuState);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _cancellationTokenSource.Cancel();
        }

        [TestMethod]
        public async Task MonitorWindowResizeAsync_ShouldCallOnResize_WhenBufferWidthChanges()
        {
            // Arrange
            _consoleWrapper.BufferWidth.Returns(100);

            // Act
            var monitorTask = _windowMonitor.MonitorWindowResizeAsync(_cancellationTokenSource.Token);
            await Task.Delay(200); // Allow some time for the monitor to detect the change

            // Assert
            _onResize.Received(1).Invoke();

            // Cleanup
            _cancellationTokenSource.Cancel();
            await monitorTask;
        }

        [TestMethod]
        public async Task MonitorWindowResizeAsync_ShouldCallOnResize_WhenBufferHeightChanges()
        {
            // Arrange
            _consoleWrapper.BufferHeight.Returns(30);

            // Act
            var monitorTask = _windowMonitor.MonitorWindowResizeAsync(_cancellationTokenSource.Token);
            await Task.Delay(200); // Allow some time for the monitor to detect the change

            // Assert
            _onResize.Received(1).Invoke();

            // Cleanup
            _cancellationTokenSource.Cancel();
            await monitorTask;
        }

        [TestMethod]
        public async Task MonitorWindowResizeAsync_ShouldNotCallOnResize_WhenBufferSizeDoesNotChange()
        {
            // Act
            var monitorTask = _windowMonitor.MonitorWindowResizeAsync(_cancellationTokenSource.Token);
            await Task.Delay(200); // Allow some time for the monitor to run

            // Assert
            _onResize.DidNotReceive().Invoke();

            // Cleanup
            _cancellationTokenSource.Cancel();
            await monitorTask;
        }

        [TestMethod]
        public async Task MonitorWindowResizeAsync_ShouldNotCallOnResize_WhenNotInMenu()
        {
            // Arrange
            _consoleWrapper.BufferWidth.Returns(100);
            _isInMenu.Invoke().Returns(false);

            // Act
            var monitorTask = _windowMonitor.MonitorWindowResizeAsync(_cancellationTokenSource.Token);
            await Task.Delay(200); // Allow some time for the monitor to detect the change

            // Assert
            _onResize.DidNotReceive().Invoke();

            // Cleanup
            _cancellationTokenSource.Cancel();
            await monitorTask;
        }
    }
}
