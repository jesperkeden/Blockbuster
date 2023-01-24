using Blockbuster.Data;
using Blockbuster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockbuster
{
    internal class Queries
    {
        internal static void Menu(Customer c)
        {
            bool queryMenu = true;
            Admin.DisplayCustomer(c);
            while (queryMenu)
            {
                Console.WriteLine("1. Where are all of our customers from?");
                Console.WriteLine("2. See our total revenue");
                Console.WriteLine("3. See which customer has the biggest collection of movies.");
                Console.WriteLine("4. See our top 3 most popular movies.");
                Console.WriteLine("5. See our top 5 best ranked movies according to IMDB.");
                Console.WriteLine("6. Return");

                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice) || choice > 6 || choice < 1)
                {
                    Console.WriteLine("Try one of the numbers in the menu!");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Queries.CustomerCity(c);
                        queryMenu = false;
                        break;
                    case 2:
                        Console.Clear();
                        Queries.TotalRevenue(c);
                        queryMenu = false;
                        break;
                    case 3:
                        Console.Clear();
                        Queries.MostStackedCustomer(c);
                        queryMenu = false;
                        break;
                    case 4:
                        Console.Clear();
                        Queries.TopThreeSold(c);
                        queryMenu = false;
                        break;
                    case 5:
                        Console.Clear();
                        Queries.ImdbTopFive(c);
                        queryMenu = false;
                        break;
                    case 6:
                        Console.Clear();
                        Menus.Show("Admin", c);
                        queryMenu = false;
                        break;
                }
            }
        }
        internal static void ImdbTopFive(Customer c)
        {
            using (var db = new Context())
            {
                var topFiveMovies = db.Movies
                    .OrderByDescending(m => m.ImdbRating)
                    .Take(5)
                    .ToList();
                foreach (var movie in topFiveMovies)
                {
                    Console.WriteLine($"Id: {movie.Id}\tThe movie '{movie.Title}' with a rating of {movie.ImdbRating}");
                }
                Console.WriteLine();
                Helpers.ReturnToQueries("This is our top 5 rated movies according to IMDB.", c);
            }
        }
        internal static void MostStackedCustomer(Customer c)
        {
            using (var db = new Context())
            {
                var customer = db.Customers
                    .Include(c => c.Movies)
                    .OrderByDescending(c => c.Movies.Count())
                    .FirstOrDefault();

                Console.WriteLine($"The customer {customer.Firstname} {customer.Lastname}, aka. '{customer.Username}' has the most movies with {customer.Movies.Count()} movies.");

                if (customer.Movies.Count > 0)
                {
                    Console.Write("Movies: ");
                    for (int i = 0; i < customer.Movies.Count; i++)
                    {
                        var movie = db.Movies.FirstOrDefault(m => m.Id == customer.Movies[i].MovieId);
                        Console.Write(movie.Title);
                        if (i != customer.Movies.Count - 1)
                        {
                            Console.Write(", ");
                        }
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("This customer has no movies.");
                }
                Console.WriteLine();
                Helpers.ReturnToQueries("", c);
            }
        }


        internal static void TopThreeSold(Customer c)
        {
            using (var db = new Context())
            {
                var top3Movies = db.Movies
                                .OrderByDescending(m => m.UnitsSold)
                                .Take(3)
                                .ToList();
                foreach (var movie in top3Movies)
                {
                    Console.WriteLine($"Id.{movie.Id}  -  The movie '{movie.Title}' has been sold {movie.UnitsSold} times.");
                }
                Console.WriteLine();
                Helpers.ReturnToQueries("This is our top 3 most popular movies.", c);
            }
        }
        internal static void TotalRevenue(Customer c)
        {
            using (var db = new Context())
            {
                var totalRevenue = db.Movies.Sum(m => m.UnitPrice * m.UnitsSold);
                var totalMoviesSold = db.Movies.Sum(m => m.UnitsSold);
                Console.WriteLine($"Total movies sold: {totalMoviesSold}");
                Console.WriteLine($"Total revenue: {totalRevenue}");
            }
            Console.WriteLine();
            Helpers.ReturnToQueries("This is our total amount of sold movies and the all-time revenue we made from sales.", c);

        }
        internal static void CustomerCity(Customer c)
        {
            using (var db = new Context())
            {
                var cities = db.Customers
                    .GroupBy(c => c.City)
                    .Select(g => new { City = g.Key, Count = g.Count() })
                    .ToList();

                foreach (var city in cities)
                {
                    Console.WriteLine($"{city.Count} customers is registred in {city.City}");
                }
                Console.WriteLine();
                Helpers.ReturnToQueries("All cities supplied by our customers and how many registred users we have in total from them.", c);
            }
        }
    }
}
