using ConsoleMenuDN;
using ConsoleMenuDN.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleMenuDN.Tests
{
    [TestClass]
    public class MenuManagerTests
    {
        private List<MenuItem> _menuItems;
        private MenuManager _menuManager;
        private IConsoleWrapper _consoleWrapper;
        private MenuRenderer _renderer;
        private IWindowMonitor _windowMonitor;
        private IKeyMonitor _keyMonitor;
        private MenuState _menuState;
        private MenuSettings _menuSettings;
        private CancellationTokenSource _cancellationTokenSource;

        [TestInitialize]
        public void Setup()
        {
            _menuItems = new List<MenuItem>
            {
                new MenuItem("Option 1", async () => await Task.Run(() => Console.WriteLine("Option 1 selected"))),
                new MenuItem("Option 2", async () => await Task.Run(() => Console.WriteLine("Option 2 selected"))),
                new MenuItem("Exit", () => Task.Run(() => Environment.Exit(0)))
            };

            _consoleWrapper = Substitute.For<IConsoleWrapper>();
            _menuState = new MenuState { ConsoleWrapper = _consoleWrapper };
            _menuSettings = new MenuSettings();
            _renderer = Substitute.For<MenuRenderer>("Main Menu", _menuItems, _menuSettings, _menuState);

            _windowMonitor = Substitute.For<IWindowMonitor>();
            _keyMonitor = Substitute.For<IKeyMonitor>();

            _cancellationTokenSource = new CancellationTokenSource();

            _menuManager = new MenuManager(_menuItems, "Main Menu", _renderer, _windowMonitor, _keyMonitor, _menuState, _menuSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _cancellationTokenSource.Cancel();
        }

        [TestMethod]
        public void UpdateSelectedItem_ShouldUpdateSelectedItem()
        {
            // Act
            _menuManager.UpdateSelectedItem(1);

            // Assert
            Assert.AreEqual(1, _menuManager.GetSelectedItem());
            _renderer.Received(1).RefreshMenu(1);
        }

        [TestMethod]
        public void ReturnToMenu_ShouldSetMenuStateToTrueAndRedrawMenu()
        {
            // Act
            _menuManager.ReturnToMenu();

            // Assert
            Assert.IsTrue(_menuManager._menuState.InMenu);
            _consoleWrapper.Received(2).Clear();
            _consoleWrapper.Received(1).SetCursorVisible(false);
            _renderer.Received(1).RedrawMenu(_menuManager.GetSelectedItem());
        }

        [TestMethod]
        public void RedrawMenu_ShouldCallRedrawMenuOnRenderer()
        {
            // Arrange
            _menuManager._menuState.InMenu = true;

            // Act
            _menuManager.RedrawMenu();

            // Assert
            _renderer.Received(1).RedrawMenu(_menuManager.GetSelectedItem());
        }

        [TestMethod]
        public async Task Run_ShouldStartMonitoringWindowResizeAndKeyInput()
        {
            // Act
            var runTask = _menuManager.Run();
            await Task.Delay(200); // Allow some time for the monitor to run

            // Assert
            await _windowMonitor.Received(1).MonitorWindowResizeAsync(Arg.Any<CancellationToken>());
            await _keyMonitor.Received(1).MonitorKeyInputAsync(Arg.Any<CancellationToken>());

            // Cleanup
            _cancellationTokenSource.Cancel();
            await runTask;
        }

        [TestMethod]
        public void Show_ShouldInitializeMenu()
        {
            // Act
            var showTask = Task.Run(() => _menuManager.Show());
            Task.Delay(500).Wait(); // Small delay to allow the menu to display

            // Assert
            _consoleWrapper.Received(2).Clear();
            _consoleWrapper.Received(1).SetCursorVisible(false);
            _renderer.Received(1).RedrawMenu(0);

            // Cleanup
            _menuManager.Cancel();
            showTask.Dispose();
        }
    }
}
