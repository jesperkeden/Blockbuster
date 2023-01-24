using Blockbuster.Models;
using Blockbuster.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Blockbuster
{
    internal static class Helpers
    {
        public static Customer TryLogin(Customer c)
        {
            Console.Clear();
            using (var db = new Context())
            {
                var customerlist = db.Customers;
                var user = GetStringFromUser("Username: ");
                var password = GetStringFromUser("Password: ");
                var correctUsername = customerlist.SingleOrDefault(x => x.Username == user);
                var correctUser = customerlist.SingleOrDefault(x => x.Password == password && x.Username == user);

                if (correctUsername != null && correctUser != null)
                {
                    c = correctUser;
                }
                else if (correctUsername != null && correctUser == null)
                {
                    ReturnToLogIn("Wrong password, try again!", c);
                }
                else
                {
                    ReturnToLogIn("User does not exist, try again or register new user!", c);
                }
                return c;
            }
        }
        public static Customer CreateUser(Customer c)
        {
            Console.Clear();
            bool loop = true;
            while (loop)
            {
                using (var db = new Context())
                {
                    var username = GetStringFromUser("Input username: ");
                    var password = GetStringFromUser("Input password: ");
                    var firstname = GetStringFromUser("Input firstname: ");
                    var lastname = GetStringFromUser("Input lastname: ");
                    var city = GetStringFromUser("Input city: ");
                    var email = GetStringFromUser("Input email: ");
                    var phone = GetStringFromUser("Input phone: ");

                    var newCustomer = new Customer()
                    {
                        Username = username,
                        Password = password,
                        Firstname = firstname,
                        Lastname = lastname,
                        City = city,
                        Email = email,
                        Phone = phone,
                    };

                    if (IsCustomerValid(newCustomer, out var errorMessage))
                    {
                        var existingCustomer = db.Customers.SingleOrDefault(x => x.Username == newCustomer.Username) != null;
                        if (existingCustomer == true)
                        {
                            Console.WriteLine("User already exists");
                            Console.WriteLine(errorMessage);
                            Console.ReadKey(true);
                            loop = false;
                            c = CreateUser(c);
                        }
                        else
                        {
                            db.Customers.Add(newCustomer);
                            try
                            {
                                db.SaveChanges();
                                Console.WriteLine("Success!");
                                c = newCustomer;
                                loop = false;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("An error occurred while saving the customer", ex);
                            }

                        }
                    }
                }
            }
            return c;
        }
        internal static bool IsCustomerValid(Customer newCustomer, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(newCustomer.Username))
            {
                errorMessage = "Username is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCustomer.Password))
            {
                errorMessage = "Password is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCustomer.Firstname))
            {
                errorMessage = "Firstname is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCustomer.Lastname))
            {
                errorMessage = "Lastname is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCustomer.City))
            {
                errorMessage = "City is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCustomer.Email))
            {
                errorMessage = "Email is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCustomer.Phone))
            {
                errorMessage = "Phone is required.";
                return false;
            }

            return true;
        }
        internal static bool IsMovieValid(Movie movie, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                errorMessage = "Title is required";
                return false;
            }

            if (movie.Duration <= 0)
            {
                errorMessage = "Duration must be greater than 0";
                return false;
            }

            if (movie.Units <= 0)
            {
                errorMessage = "Units must be greater than 0";
                return false;
            }

            if (movie.UnitPrice < 0)
            {
                errorMessage = "UnitPrice must be greater than or equal to 0";
                return false;
            }

            if (movie.ImdbRating < 0 || movie.ImdbRating > 10)
            {
                errorMessage = "ImdbRating must be between 0 and 10";
                return false;
            }
            errorMessage = "";
            return true;
        }


        internal static void ReturnToUpdateCustomer(string message, Customer c)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to previous menu");
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
            Menus.Show("Admin", c);
        }
        internal static void ReturnToMain(string message, Customer c)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to previous menu");
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
            Menus.Show("Main", c);
        }
        internal static void ReturnToLogIn(string message, Customer c)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
            Menus.Show("LogIn", c);
        }
        internal static void ReturnToAdmin(string message, Customer c)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
            Menus.Show("Admin", c);
        }
        internal static void ReturnToQueries(string message, Customer c)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
            Queries.Menu(c);
        }


        public static string GetStringFromUser(string prompt)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                Console.Write("Input can't be empty, please enter a valid value: ");
                input = Console.ReadLine();
            }
            return input;
        }
        public static int GetIntFromUser(string prompt)
        {
            Console.Write(prompt);
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input, please enter a valid number: ");
            }
            return input;
        }
        public static double GetDoubleFromUser(string prompt)
        {
            Console.Write(prompt);
            double input;
            while (!double.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input, please enter a valid number: ");
            }
            return input;
        }
        public static decimal GetDecimalFromUser(string prompt)
        {
            Console.Write(prompt);
            decimal input;
            while (!decimal.TryParse(Console.ReadLine(), out input))
            {
                Console.Write("Invalid input, please enter a valid decimal number: ");
            }
            return input;
        }
        public static int TryNumber(int maxValue, int minValue)
        {
            int number = 0;
            bool correctInput = false;
            while (!correctInput)
            {
                if (!int.TryParse(Console.ReadLine(), out number) || number > maxValue || number < minValue)
                {
                    Console.Write("Wrong input, try again: ");

                }
                else
                {
                    correctInput = true;
                }
            }
            return number;
        }
        internal static void ImportToDatabase()
        {
            using (var db = new Context())
            {
                var genresToAdd = new List<Genre>()
                    {
                        new Genre { Name = "Action" },    // GenreID = 1
                        new Genre { Name = "Crime" },     // GenreID = 2
                        new Genre { Name = "Drama" },     // GenreID = 3
                        new Genre { Name = "Comedy" },    // GenreID = 4
                        new Genre { Name = "Adventure" }, // GenreID = 5
                        new Genre { Name = "Western" },   // GenreID = 6
                        new Genre { Name = "SciFi" },     // GenreID = 7
                        new Genre { Name = "Fantasy" },   // GenreID = 8  
                        new Genre { Name = "Thriller" },  // GenreID = 9
                        new Genre { Name = "Romance" },   // GenreID = 10
                        new Genre { Name = "Animation" }, // GenreID = 11
                        new Genre { Name = "Family" },    // GenreID = 12
                        new Genre { Name = "Horror" },    // GenreID = 13
                        new Genre { Name = "Mystery" },   // GenreID = 14
                        new Genre { Name = "Musical" },   // GenreID = 15
                        new Genre { Name = "Sports" },    // GenreID = 16
                        new Genre { Name = "War" },       // GenreID = 17
                        new Genre { Name = "Music" },     // GenreID = 18
                        new Genre { Name = "History" },   // GenreID = 19

                    };

                try
                {
                    db.Genres.AddRange(genresToAdd);
                    db.SaveChanges();
                    Console.WriteLine("Genres added successfully!");
                    Console.WriteLine();
                    Console.WriteLine("Now time for some movies..");
                    ImportMoviesToDatabase();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while trying to import the movies.");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        internal static void ImportMoviesToDatabase()
        {
            using (var db = new Context())
            {
                Random rnd = new Random();
                List<Movie> movies = new List<Movie>();

                Movie movie = new Movie();
                movie.Title = "The Dark Knight";
                movie.Duration = 152;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 9.0;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Inception";
                movie.Duration = 148;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.8;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Lord of the Rings: The Fellowship of the Ring";
                movie.Duration = 178;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.8;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Lord of the Rings: The Return of the King";
                movie.Duration = 201;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 9.0;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movies.Add(movie);


                movie = new Movie();
                movie.Title = "The Lord of the Rings: The Two Towers";
                movie.Duration = 179;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.8;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "777 Charlie";
                movie.Duration = 164;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.9;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Good, the Bad and the Ugly";
                movie.Duration = 161;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.8;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Western
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Star Wars - The Empire Strikes Back";
                movie.Duration = 124;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.7;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 8 });  // Fantasy
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Interstellar";
                movie.Duration = 169;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Star Wars";
                movie.Duration = 121;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 8 });  // Fantasy
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Chaos Class";
                movie.Duration = 87;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 9.2;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Life is beautiful";
                movie.Duration = 116;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Back to the Future";
                movie.Duration = 116;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Intouchables";
                movie.Duration = 116;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Modern Times";
                movie.Duration = 87;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "It's a Wonderful Life";
                movie.Duration = 130;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Green Mile";
                movie.Duration = 189;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 8 });  // Fantasy
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Spirited Away";
                movie.Duration = 125;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 11 }); // Animation
                movie.Genres.Add(new MovieGenre { GenreId = 12 }); // Family
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Spider-Man: Into the Spider-Verse";
                movie.Duration = 117;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 11 }); // Animation
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Coco";
                movie.Duration = 105;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 11 }); // Animation
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Alien";
                movie.Duration = 117;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movie.Genres.Add(new MovieGenre { GenreId = 13 }); // Horror
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Psycho";
                movie.Duration = 109;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 9 });  // Thriller
                movie.Genres.Add(new MovieGenre { GenreId = 13 }); // Horror
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Shining";
                movie.Duration = 146;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 13 }); // Horror
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Thing";
                movie.Duration = 109;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movie.Genres.Add(new MovieGenre { GenreId = 13 }); // Horror
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Tumbbad";
                movie.Duration = 104;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.2;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 8 });  // Fantasy
                movie.Genres.Add(new MovieGenre { GenreId = 13 }); // Horror
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Mirror Game";
                movie.Duration = 147;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 9.0;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 9 });  // Thriller
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Jai Bhim";
                movie.Duration = 164;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.8;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Seven";
                movie.Duration = 127;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Sita Ramam";
                movie.Duration = 163;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Harakiri";
                movie.Duration = 133;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Your Name";
                movie.Duration = 106;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 8 });  // Fantasy
                movie.Genres.Add(new MovieGenre { GenreId = 11 }); // Animation
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Good Will Hunting";
                movie.Duration = 126;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.3;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Good Will Hunting";
                movie.Duration = 108;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.3;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Singing in the Rain";
                movie.Duration = 103;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.3;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movie.Genres.Add(new MovieGenre { GenreId = 15 }); // Musical
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Amelie";
                movie.Duration = 122;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.3;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 4 });  // Comedy
                movie.Genres.Add(new MovieGenre { GenreId = 10 }); // Romance
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Matrix";
                movie.Duration = 136;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.7;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Terminator 2: Judgement Day";
                movie.Duration = 137;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Prestige";
                movie.Duration = 130;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Avengers: Endgame";
                movie.Duration = 181;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Avengers: Infinity War";
                movie.Duration = 149;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 7 });  // SciFi
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Dangal";
                movie.Duration = 161;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.3;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Warrior";
                movie.Duration = 140;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Raging Bull";
                movie.Duration = 129;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.2;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 16 }); // Sports
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Children of Heaven";
                movie.Duration = 89;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.2;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 12 }); // Family
                movie.Genres.Add(new MovieGenre { GenreId = 16 }); // Sports
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Rocky";
                movie.Duration = 120;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.1;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 16 }); // Sports
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Silence of the Lambs";
                movie.Duration = 118;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 9 });  // Thriller
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Léon";
                movie.Duration = 110;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 1 });  // Action
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Parasite";
                movie.Duration = 132;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 9 });  // Thriller
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Departed";
                movie.Duration = 151;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 9 });  // Thriller
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Usual Suspects";
                movie.Duration = 106;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 2 });  // Crime
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Saving Private Ryan";
                movie.Duration = 169;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.6;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 17 }); // War
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Apocalypse Now";
                movie.Duration = 147;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 14 }); // Mystery
                movie.Genres.Add(new MovieGenre { GenreId = 17 }); // War
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Pianist";
                movie.Duration = 150;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 18 }); // Music
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Grave of the Fireflies";
                movie.Duration = 89;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 11 }); // Animation
                movie.Genres.Add(new MovieGenre { GenreId = 17 }); // War
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Braveheart";
                movie.Duration = 178;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 19 }); // History
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Once upon a time in the west";
                movie.Duration = 165;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.5;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 6 });  // Western
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Django Unchained";
                movie.Duration = 165;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 6 });  // Western
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "Unforgiven";
                movie.Duration = 130;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.4;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 6 });  // Western
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "For a few dollars more";
                movie.Duration = 132;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.2;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 6 });  // Western
                movies.Add(movie);

                movie = new Movie();
                movie.Title = "The Treasure of the Sierra Madre";
                movie.Duration = 126;
                movie.Units = rnd.Next(1, 10);
                movie.UnitPrice = rnd.Next(99, 299);
                movie.ImdbRating = 8.2;
                movie.Genres = new List<MovieGenre>();
                movie.Genres.Add(new MovieGenre { GenreId = 3 });  // Drama
                movie.Genres.Add(new MovieGenre { GenreId = 5 });  // Adventure
                movie.Genres.Add(new MovieGenre { GenreId = 6 });  // Western
                movies.Add(movie);

                try
                {
                    db.Movies.AddRange(movies);
                    db.SaveChanges();
                    Console.WriteLine("Movies added successfully!");
                    Console.WriteLine();
                    Console.WriteLine("and finally, its time to add a user called Admin");
                    ImportAdminToDatabase();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while trying to import the movies.");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        internal static void ImportAdminToDatabase()
        {
            using (var db = new Context())
            {
                List<Customer> customers = new List<Customer>();

                Customer admin = new Customer();
                admin.Username = "admin";
                admin.Password = "admin";
                admin.Firstname = "Jesper";
                admin.Lastname = "Kedén";
                admin.City = "Oxelösund";
                admin.Email = "jesper.keden@hotmail.com";
                admin.Phone = "0708759983";
                customers.Add(admin);

                try
                {
                    db.Customers.AddRange(admin);
                    db.SaveChanges();
                    Console.WriteLine("Admin added successfully!");
                    Console.WriteLine();
                    Console.WriteLine("Time to log in..");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while trying to import the movies.");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
