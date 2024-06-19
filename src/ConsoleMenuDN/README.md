# ConsoleMenuDN

ConsoleMenuDN is a helper library for creating interactive menus within console applications. This library simplifies the process of building and managing console-based menus, making it easier to create user-friendly command-line interfaces.

## Features

- Easy-to-use API for creating console menus.
- Support for asynchronous menu actions.
- Automatic handling of key inputs (navigation, selection).
- Responsive to console window resize events.

## Installation

You can install the ConsoleMenuDN package via NuGet:

```sh
dotnet add package ConsoleMenuDN
```

## Usage

### Basic Example

Here's a simple example of how to use ConsoleMenuDN to create a console menu:

```
using ConsoleMenuDN;

class Program
{
    static async Task Main(string[] args)
    {
        var menuOptions = new List<MenuOption>
        {
            new MenuOption("Option 1", async () => await Task.Run(() => Console.WriteLine("Option 1 selected"))),
            new MenuOption("Option 2", async () => await Task.Run(() => Console.WriteLine("Option 2 selected"))),
            new MenuOption("Exit", async () => Environment.Exit(0))
        };

        var menu = new MenuManager(menuOptions, "Main Menu");
        menu.Show();
    }
}
```

### Creating Menu Options

Menu options can be created by instantiating the MenuOption class. Each option requires a name and an action to perform when selected:

```
var option = new MenuOption("Option 1", async () => await Task.Run(() => Console.WriteLine("Option 1 selected")));
```

### Displaying the Menu

To display the menu, create an instance of MenuManager and call the Show method:

```
var menu = new MenuManager(menuOptions, "Main Menu");
menu.Show();
```


## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.