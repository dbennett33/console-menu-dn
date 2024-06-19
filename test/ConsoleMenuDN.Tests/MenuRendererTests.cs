using ConsoleMenuDN;
using ConsoleMenuDN.Interfaces;
using NSubstitute;

namespace ConsoleMenuDN.Tests
{
    [TestClass]
    public class MenuRendererTests
    {
        private List<MenuItem> _menuItems;
        private MenuRenderer _menuRenderer;
        private IConsoleWrapper _consoleWrapper;
        private MenuState _menuState;
        private MenuSettings _menuSettings;

        [TestInitialize]
        public void Setup()
        {
            _menuItems = new List<MenuItem>
            {
                new MenuItem("Option 1", async () => await Task.Run(() => Console.WriteLine("Option 1 selected"))),
                new MenuItem("Option 2", async () => await Task.Run(() => Console.WriteLine("Option 2 selected"))),
                new MenuItem("Exit", async () => Task.Run(() => Environment.Exit(0)))
            };

            _consoleWrapper = Substitute.For<IConsoleWrapper>();
            _menuState = new MenuState { ConsoleWrapper = _consoleWrapper };
            _menuSettings = new MenuSettings();

            _menuRenderer = new MenuRenderer("Main Menu", _menuItems, _menuSettings, _menuState);
        }

        [TestMethod]
        public void RedrawMenu_ShouldClearConsoleAndDrawHeaderAndMenu()
        {
            // Act
            _menuRenderer.RedrawMenu(1);

            // Assert
            _consoleWrapper.Received(1).Clear();
            _consoleWrapper.Received(1).WriteLine("\x1b[3J");
            _consoleWrapper.Received(8).SetCursorPosition(Arg.Any<int>(), Arg.Any<int>());
        }

        [TestMethod]
        public void RefreshMenu_ShouldSetConsoleColorsAndDrawMenuItems()
        {
            // Act
            _menuRenderer.RefreshMenu(1);

            // Assert
            foreach (var mo in _menuItems)
            {
                _consoleWrapper.Received().SetCursorPosition(mo.XStartPos, mo.YStartPos);
                _consoleWrapper.Received().Write(Arg.Any<string>());
            }
        }

        [TestMethod]
        public void DrawMenu_ShouldSetMenuItemsPositionsAndDrawThem()
        {
            // Act
            _menuRenderer.DrawMenu(1);

            // Assert
            foreach (var mo in _menuItems)
            {
                _consoleWrapper.Received().SetCursorPosition(mo.XStartPos, mo.YStartPos);
                _consoleWrapper.Received().Write(Arg.Any<string>());
            }
        }

        [TestMethod]
        public void GetDisplayName_ShouldReturnCorrectDisplayName()
        {
            // Arrange
            _menuSettings.ShowLineNumbers = true;

            // Act
            var displayName = _menuRenderer.GetDisplayName(_menuItems[0]);

            // Assert
            Assert.AreEqual("(1) Option 1", displayName);
        }

        [TestMethod]
        public void DrawHeader_ShouldDrawHeaderAndTitle()
        {
            // Act
            _menuRenderer.DrawHeader();

            // Assert
            for (int i = 0; i < _menuState.ConsoleWrapper.BufferWidth; i++)
            {
                _consoleWrapper.Received().SetCursorPosition(i, 0);
                _consoleWrapper.Received().Write("=");
                _consoleWrapper.Received().SetCursorPosition(i, 2);
                _consoleWrapper.Received().Write("=");
            }

            _consoleWrapper.Received().SetCursorPosition(Arg.Any<int>(), 1);
            _consoleWrapper.Received().Write("Main Menu");
        }

        [TestMethod]
        public void Draw_ShouldSetCursorPositionAndWrite()
        {
            // Act
            _menuRenderer.Draw("Test", 1, 1);

            // Assert
            _consoleWrapper.Received(1).SetCursorPosition(1, 1);
            _consoleWrapper.Received(1).Write("Test");
        }

        [TestMethod]
        public void GetCentreX_ShouldReturnCentreX()
        {
            // Act
            var centreX = _menuRenderer._centreX;

            // Assert
            Assert.AreEqual(_menuState.ConsoleWrapper.WindowWidth / 2, centreX);
        }

        [TestMethod]
        public void GetMenuOptions_ShouldReturnMenuOptions()
        {
            // Act
            var menuOptions = _menuRenderer._menuOptions;

            // Assert
            Assert.AreEqual(_menuItems.Count, menuOptions.Count);
        }

        [TestMethod]
        public void GetMenuSettings_ShouldReturnMenuSettings()
        {
            // Act
            var menuSettings = _menuRenderer._menuSettings;

            // Assert
            Assert.AreEqual(_menuSettings, menuSettings);
        }

        [TestMethod]
        public void GetMenuState_ShouldReturnMenuState()
        {
            // Act
            var menuState = _menuRenderer._menuState;

            // Assert
            Assert.AreEqual(_menuState, menuState);
        }

        [TestMethod]
        public void GetTitle_ShouldReturnTitle()
        {
            // Act
            var title = _menuRenderer._title;

            // Assert
            Assert.AreEqual("Main Menu", title);
        } 
    }
}
