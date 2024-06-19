using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ConsoleMenuDN;

namespace ConsoleMenuDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            MainMenu();
        }

        public static void MainMenu()
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
 
        public static void NestedMenu()
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
