# ConsoleMenuDN

[![.NET](https://github.com/dbennett33/console-menu-dn/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dbennett33/console-menu-dn/actions/workflows/dotnet.yml)
https://github.com/dbennett33/console-menu-dn

ConsoleMenuDN is a helper library for creating interactive menus within console applications. This library simplifies the process of building and managing console-based menus, making it easier to create user-friendly command-line interfaces.


![image](https://github.com/dbennett33/console-menu-dn/assets/17428180/4614ea12-654b-438d-8412-75771d0b8e33)





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

```c#
using ConsoleMenuDN;

class Program
{
    static void Main(string[] args)
    {
        MainMenu();
    }

    private static void MainMenu()
    {
        List<MenuItem> menuOptions = new List<MenuItem>
        {
            new MenuItem("Fetch something", () => Console.WriteLine("Fetching")),
            new MenuItem("Process something", () => Console.WriteLine("Processing")),
            new MenuItem("Save something", () => Console.WriteLine("Saving")),  
            new MenuItem("Exit", () => Environment.Exit(0))
        }; 

        var menu = new MenuManager(menuOptions, "Menu");
        menu.Show();
    }
}
```



### Displaying the Menu

To display the menu, create an instance of MenuManager and call the Show method:

```c#
var menu = new MenuManager(menuOptions, "Main Menu");
menu.Show();
```


### Creating Sub-Menus

To use a sub-menu, create another instance of MenuManager with the desired MenuItems and call this from the parent menu.

ConsoleMenuDN isn't smart, you need to manage 'return to previous menu' options yourself. You can create a sub-menu with the ability to return to its parent by following this pattern:

```c#
using ConsoleMenuDN;

namespace ConsoleMenuDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainMenu();
        }

        private static void MainMenu()
        {
            List<MenuItem> menuOptions = new List<MenuItem>
            {
                new MenuItem("Fetch something", () => Console.WriteLine("Fetching")),
                new MenuItem("Process something", () => Console.WriteLine("Processing")),
                new MenuItem("Save something", () => Console.WriteLine("Saving")),
                new MenuItem("A Sub-Menu", () => NestedMenu()),  
                new MenuItem("Exit", () => Environment.Exit(0))
            }; 

            var menu = new MenuManager(menuOptions, "Menu");
            menu.Show();
        }
 
        private static void NestedMenu()
        {
            List<MenuItem> menuOptions = new List<MenuItem>
            {
                new MenuItem("Return to Main Menu", () =>  MainMenu()),
                new MenuItem("Execute task A", () => Console.WriteLine("Executing Task A")),
                new MenuItem("Execute task B", () => Console.WriteLine("Executing Task B"))              
            };

            var secondMenu = new MenuManager(menuOptions, "A Sub-Menu");
            secondMenu.Show();
        }        
    }
}
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the GNU General Public License v3.0. See the LICENSE file for details.
