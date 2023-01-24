using Blockbuster.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockbuster
{
    internal class Menus
    {
        internal static void RunShow()
        {
            bool runProgram = true;
            Customer? c = null;
            while (runProgram)
            {
                Console.Clear();
                Console.ResetColor();

                if (c == null)
                {
                    Console.Clear();
                    c = Menus.Show("LogIn", c);
                }
                else
                {
                    Console.Clear();
                    Admin.DisplayCustomer(c);
                    if (c.Username == "admin")
                    {
                        Console.Clear();
                        Menus.Show("Main", c);
                    }
                    else
                    {
                        Console.Clear();
                        Admin.DisplayCustomer(c);
                        Menus.Show("Main", c);
                    }
                }
            }
        }
        public static Customer Show(string value, Customer c)
        {
            bool logIn = true;
            bool goMain = true;
            bool adminMenu = true;

            if (value == "Main")
            {
                Admin.DisplayCustomer(c);
                while (goMain)
                {
                    Console.WriteLine("1. Browse Movies");
                    Console.WriteLine("2. View your profile");
                    Console.WriteLine("3. Admin Menu");
                    Console.WriteLine("4. Log Out");

                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice) || choice > 4 || choice < 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Try one of the numbers in the menu!");
                        continue;
                    }

                    switch (input)
                    {
                        case "1":
                            Console.Clear();
                            Admin.BrowseAllGenres(c);
                            Console.ReadKey();
                            goMain = false;
                            break;
                        case "2":
                            Console.Clear();
                            Admin.OneCustomer(c);
                            Console.ReadKey();
                            goMain = false;
                            break;
                        case "3":
                            Console.Clear();
                            if (c.Username == "admin")
                            {
                                Show("Admin", c);
                            }
                            else
                            {
                                Console.WriteLine("You cant do that!");
                            }
                            break;
                        case "4":
                            Console.Clear();
                            c = null;
                            goMain = false;
                            Show("LogIn", c);
                            break;
                        default:
                            Console.WriteLine("Try one of the numbers in the menu!");
                            break;
                    }
                }
            }
            if (value == "LogIn")
            {
                while (logIn)
                {
                    Console.WriteLine("1. Sign In");
                    Console.WriteLine("2. Create New Customer");
                    Console.WriteLine("3. Exit");

                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice) || choice > 3 || choice < 1)
                    {
                        Console.WriteLine("Try one of the numbers in the menu!");
                        continue;
                    }
                    switch (choice)
                    {
                        case 1:
                            c = Helpers.TryLogin(c);
                            logIn = false;
                            break;
                        case 2:
                            c = Helpers.CreateUser(c);
                            logIn = false;
                            break;
                        case 3:
                            logIn = false;
                            break;
                    }
                    Console.Clear();
                    Show("Main", c);
                }
            }

            if (value == "Admin")
            {
                Admin.DisplayCustomer(c);
                while (adminMenu)
                {
                    Console.WriteLine("1. Add movie");
                    Console.WriteLine("2. Edit movies");
                    Console.WriteLine("3. Delete movies");
                    Console.WriteLine("4. See all movies");
                    Console.WriteLine("5. Edit customers");
                    Console.WriteLine("6. Queries");
                    Console.WriteLine("7. Return");

                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice) || choice > 7 || choice < 1)
                    {
                        Console.WriteLine("Try one of the numbers in the menu!");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            Admin.AddNewMovie(c);
                            adminMenu = false;
                            break;
                        case 2:
                            Admin.UpdateMovie(c);
                            adminMenu = false;
                            break;
                        case 3:
                            Admin.DeleteMovies(c); 
                            adminMenu = false;
                            break;
                        case 4:
                            Admin.BrowseAllMovies(c); 
                            adminMenu = false;
                            break;
                        case 5:
                            Admin.BrowseAllCustomers(c);
                            adminMenu = false;
                            break;
                            break;
                        case 6:
                            Console.Clear();
                            Queries.Menu(c);
                            adminMenu = false;
                            break;
                        case 7:
                            Console.Clear();
                            Show("Main", c);
                            adminMenu = false;
                            break;
                    }
                }
            }
            return c;
        }
    }
}
