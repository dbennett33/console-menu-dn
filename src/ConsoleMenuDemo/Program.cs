using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            List<MenuOption> menuOptions = new List<MenuOption>
            {
                new MenuOption("A Nested Menu", () => NestedMenu()),
                new MenuOption("Option 2", () => Console.WriteLine("Executing Option 2")),
                new MenuOption("Option 3", () => Console.WriteLine("Executing Option 3")),
                new MenuOption("Async Option", async () =>
                {
                    await Task.Delay(1000); // Simulate async work
                    Console.WriteLine("Executing Async Option");
                })
            };

            var menu = new ConsoleMenu(menuOptions, "Main Menu");
            menu.Show();
        }


        private static void NestedMenu()
        {
            List<MenuOption> menuOptions = new List<MenuOption>
            {
                new MenuOption("..", () =>  MainMenu()),
                new MenuOption("Nested Menu Option 2", () => Console.WriteLine("Executing Option 2")),
                new MenuOption("Nested Menu Option 3", () => Console.WriteLine("Executing Option 3"))              
            };

            var menu = new ConsoleMenu(menuOptions, "Second Menu");
            menu.Show();
        }
    }
}
