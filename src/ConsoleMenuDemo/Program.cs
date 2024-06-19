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
            List<MenuItem> menuOptions = new List<MenuItem>
            {
                new MenuItem("Option 1 With more text", () => NestedMenu()),
                new MenuItem("Option 2 some extra", () => Console.WriteLine("Executing Option 2")),
                new MenuItem("Option 3 lots of text", () => Console.WriteLine("Executing Option 3")),
                new MenuItem("Async Option", async () =>
                {
                    await Task.Delay(1000); // Simulate async work
                    Console.WriteLine("Executing Async Option");
                }),
                new MenuItem("Exit", () => Environment.Exit(0))
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
            List<MenuItem> menuOptions = new List<MenuItem>
            {
                new MenuItem("..", () =>  MainMenu()),
                new MenuItem("Nested Menu Option 2", () => Console.WriteLine("Executing Option 2")),
                new MenuItem("Nested Menu Option 3", () => Console.WriteLine("Executing Option 3"))              
            };

            var secondMenu = new MenuManager(menuOptions, "Second Menu");
            secondMenu.Show();
        }        
    }
}
