using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                new MenuOption("Option 1 With more text", () => NestedMenu()),
                new MenuOption("Option 2 some extra", () => Console.WriteLine("Executing Option 2")),
                new MenuOption("Option 3 lots and lots of text", () => Console.WriteLine("Executing Option 3")),
                new MenuOption("Async Option", async () =>
                {
                    await Task.Delay(1000); // Simulate async work
                    Console.WriteLine("Executing Async Option");
                }),
                new MenuOption("Exit", () => Environment.Exit(0))
            }; 

            var menu = new MenuManager(menuOptions, "Menu");
            menu.Show();
        }

        private static void Option1()
        {
            Console.WriteLine("Option 1 selected, please enter a value:");
            Console.Read();
        }


        private static void NestedMenu()
        {
            List<MenuOption> menuOptions = new List<MenuOption>
            {
                new MenuOption("..", () =>  MainMenu()),
                new MenuOption("Nested Menu Option 2", () => Console.WriteLine("Executing Option 2")),
                new MenuOption("Nested Menu Option 3", () => Console.WriteLine("Executing Option 3"))              
            };

            var secondMenu = new MenuManager(menuOptions, "Second Menu");
            secondMenu.Show();
        }        
    }
}
