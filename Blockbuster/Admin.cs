using Blockbuster.Models;
using Blockbuster.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blockbuster
{
    internal class Admin
    {
        public static void AddNewMovie(Customer c)
        {
            var movie = new Movie();

            movie.Title = Helpers.GetStringFromUser("Enter Title: ");
            movie.Duration = Helpers.GetIntFromUser("Enter the length(minutes) of the movie: ");
            movie.Units = Helpers.GetIntFromUser("Enter the amount of units you'd wish to add: ");
            movie.UnitPrice = Helpers.GetDecimalFromUser("Enter the retail price for one unit: ");
            movie.ImdbRating = Helpers.GetDoubleFromUser("Enter the IMDB rating of the movie: ");
            movie.Genres = new List<MovieGenre>();

            using (var db = new Context())
            {
                // Get all existing genres from the database
                var genres = db.Genres.ToList();
                var genreList = new List<Genre>();

                // Allow the user to select the genres for the movie
                while (true)
                {
                    Console.WriteLine("Select genres for the movie (Enter the Ids of the genre separated by comma/blankspace or 0 to finish):");
                    for (int i = 0; i < genres.Count; i++)
                    {
                        Console.WriteLine($"{genres[i].Id}. {genres[i].Name}");
                    }

                    // Get the user's selection
                    string input = Console.ReadLine();

                    if (input == "0")
                    {
                        // If the user enters 0, exit the loop
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        // Otherwise, add the selected genre to the movie
                        string[] selections = input.Split(',', ' ');
                        foreach (string sel in selections)
                        {
                            int selection;
                            if (int.TryParse(sel, out selection) && selection > 0 && selection <= genres.Count)
                            {
                                genreList.Add(genres[selection - 1]);
                                movie.Genres.Add(new MovieGenre { Genre = genres[selection - 1], GenreId = genres[selection - 1].Id, Movie = movie, MovieId = movie.Id });
                            }
                        }
                    }
                }


                if (Helpers.IsMovieValid(movie, out var errorMessage))
                {
                    db.Movies.Add(movie);
                    try
                    {
                        db.SaveChanges();
                        Helpers.ReturnToAdmin($"Successfully added the movie {movie.Title}.", c);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while saving the movie" + errorMessage, ex);
                    }
                }
            }
        }
        internal static void BrowseAllGenres(Customer c)
        {
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                var genreList = db.Genres.ToList();

                for (int i = 0; i < genreList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {genreList[i].Name}");
                }
                Console.WriteLine();
                var selection = Helpers.GetIntFromUser("Select a genre to browse: ");
                if (selection <= 0 || selection > genreList.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }
                var selectedGenre = genreList[selection - 1];
                int genreId = selectedGenre.Id;
                OneGenre(genreId, c);
            }
        }
        internal static void OneGenre(int genreId, Customer c)
        {
            Console.Clear();
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                var selectedGenre = db.Genres.Where(x => x.Id == genreId).FirstOrDefault();
                if (selectedGenre != null)
                {
                    var movieList = db.Movies.Include(m => m.Genres).Where(m => m.Genres.Any(g => g.GenreId == genreId)).ToList();
                    if (movieList.Count > 0)
                    {
                        for (int i = 0; i < movieList.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}: {movieList[i].Title}");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Enter the id of the movie you'd like to see more information about: ");
                        int answer = Helpers.TryNumber(movieList.Count, 1);
                        int movieId = movieList[answer - 1].Id;
                        OneMovieCustomerDisplay(movieId, c);
                    }
                    else
                    {
                        Console.WriteLine("No movies found in this genre");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid genre id");
                }
            }
        }
        internal static void OneMovieCustomerDisplay(int movieId, Customer c)
        {
            Console.Clear();
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                var selectedMovie = db.Movies.Where(m => m.Id == movieId).FirstOrDefault();
                if (selectedMovie != null)
                {
                    Console.WriteLine($"Title: {selectedMovie.Title}");
                    Console.WriteLine($"Duration: {selectedMovie.Duration}");
                    Console.WriteLine($"Units: {selectedMovie.Units}");
                    Console.WriteLine($"Unit price: {selectedMovie.UnitPrice}");
                    Console.WriteLine($"IMDB rating: {selectedMovie.ImdbRating}");
                    var selectedMovieGenres = db.MovieGenres.Where(mg => mg.MovieId == movieId).ToList();
                    if (selectedMovieGenres.Count > 0)
                    {
                        Console.Write("Genres: ");
                        for (int i = 0; i < selectedMovieGenres.Count; i++)
                        {
                            var movieGenre = selectedMovieGenres[i];
                            var genre = db.Genres.Where(g => g.Id == movieGenre.GenreId).FirstOrDefault();
                            Console.Write($"{genre.Name}");
                            if (i != selectedMovieGenres.Count - 1) //check if it is the last genre in the list
                            {
                                Console.Write(", ");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    Console.WriteLine("1. Buy movie");
                    Console.WriteLine("2. Return to main menu");
                    Console.Write("What would you like to do next? ");
                    int selection = Helpers.TryNumber(2, 1);
                    if (selection == 1)
                    {
                        BuyMovie(movieId, c);
                    }
                    else if (selection == 2)
                    {
                        Menus.Show("Main", c);
                    }
                }
            }
        }
        internal static void OneMovieAdminDisplay(int movieId, Customer c)
        {
            Console.Clear();
            Admin.DisplayCustomer(c);

            using (var db = new Context())
            {
                var selectedMovie = db.Movies.Where(m => m.Id == movieId).FirstOrDefault();
                if (selectedMovie != null)
                {
                    Console.WriteLine($"Title: {selectedMovie.Title}");
                    Console.WriteLine($"Duration: {selectedMovie.Duration}");
                    Console.WriteLine($"Units: {selectedMovie.Units}");
                    Console.WriteLine($"Unit price: {selectedMovie.UnitPrice}");
                    Console.WriteLine($"IMDB rating: {selectedMovie.ImdbRating}");
                    var selectedMovieGenres = db.MovieGenres.Where(mg => mg.MovieId == movieId).ToList();
                    if (selectedMovieGenres.Count > 0)
                    {
                        Console.Write("Genres: ");
                        for (int i = 0; i < selectedMovieGenres.Count; i++)
                        {
                            var genre = db.Genres.Where(g => g.Id == selectedMovieGenres[i].GenreId).FirstOrDefault();
                            if (i == selectedMovieGenres.Count - 1)
                                Console.Write(genre.Name);
                            else
                                Console.Write($"{genre.Name}, ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    Helpers.ReturnToMain("Above is a detailed summary of the movie.", c);
                }
            }
        }
        internal static void BuyMovie(int movieId, Customer c)
        {
            using (var db = new Context())
            {
                var success = true;
                var selectedMovie = db.Movies.Where(m => m.Id == movieId).FirstOrDefault();
                if (selectedMovie != null)
                {
                    if (selectedMovie.Units > 0)
                    {
                        db.CustomerMovies.Add(new CustomerMovie { CustomerId = c.Id, MovieId = movieId });
                        selectedMovie.UnitsSold++;
                        selectedMovie.Units--;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            Console.WriteLine("An error occurred while trying to buy the movie, please try again later.");
                            Console.WriteLine(ex.Message);
                        }
                        if (success)
                        {
                            Console.WriteLine("Movie bought successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry, this movie is out of stock.");
                    }
                }
                Helpers.ReturnToMain("", c);
            }
        }


        internal static void BrowseAllMovies(Customer c)
        {
            Console.Clear();
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                var movies = db.Movies.ToList();
                if (movies.Count == 0)
                {
                    Console.WriteLine("No movies found");
                    return;
                }
                for (int i = 0; i < movies.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {movies[i].Title}");
                }
                Console.WriteLine();
                Console.Write("Enter the number of the movie you'd like to see more information about: ");
                int selection = Helpers.TryNumber(movies.Count, 1);
                int movieId = movies[selection - 1].Id;
                OneMovieAdminDisplay(movieId, c);
            }
        }
        internal static void BrowseAllCustomers(Customer c)
        {
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                var customers = db.Customers.ToList();
                if (customers.Count == 0)
                {
                    Console.WriteLine("No customers found");
                    return;
                }
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Id: {customer.Id}\tUsername: {customer.Username}");
                }
                Console.WriteLine();
                var selection = Helpers.GetIntFromUser("Select a customer to update: ");
                if (selection <= 0 || selection > customers.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }
                var selectedCustomer = customers[selection - 1];

                Console.Clear();
                Console.WriteLine("What would you like to update?");
                Console.WriteLine("1.Username");
                Console.WriteLine("2.Password");
                Console.WriteLine("3.Firstname");
                Console.WriteLine("4.Lastname");
                Console.WriteLine("5.City");
                Console.WriteLine("6.Email");
                Console.WriteLine("7.Phone");
                Console.WriteLine();

                var propertySelection = Helpers.GetIntFromUser("What would you like to update: ");
                switch (propertySelection)
                {
                    case 1:
                        selectedCustomer.Username = Helpers.GetStringFromUser("Enter new username: ");
                        break;
                    case 2:
                        selectedCustomer.Password = Helpers.GetStringFromUser("Enter new password: ");
                        break;
                    case 3:
                        selectedCustomer.Firstname = Helpers.GetStringFromUser("Enter new firstname: ");
                        break;
                    case 4:
                        selectedCustomer.Lastname = Helpers.GetStringFromUser("Enter new lastname: ");
                        break;
                    case 5:
                        selectedCustomer.City = Helpers.GetStringFromUser("Enter new city: ");
                        break;
                    case 6:
                        selectedCustomer.Email = Helpers.GetStringFromUser("Enter new email: ");
                        break;
                    case 7:
                        selectedCustomer.Phone = Helpers.GetStringFromUser("Enter new phonenumber: ");
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        return;
                }
                try
                {
                    db.SaveChanges();
                    Console.WriteLine();
                    Helpers.ReturnToUpdateCustomer("Customer updated successfully.", c);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the customer", ex);
                }
            }
        }
        internal static void UpdateMovie(Customer c)
        {
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                var movies = db.Movies.ToList();
                if (movies.Count == 0)
                {
                    Console.WriteLine("No movies found");
                    return;
                }
                for (int i = 0; i < movies.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {movies[i].Title}");
                }
                Console.WriteLine();
                var selection = Helpers.GetIntFromUser("Select a movie to update: ");
                if (selection <= 0 || selection > movies.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }
                var selectedMovie = movies[selection - 1];

                Console.Clear();
                Console.WriteLine("What would you like to update?");
                Console.WriteLine("1.Title");
                Console.WriteLine("2.Duration");
                Console.WriteLine("3.Units");
                Console.WriteLine("4.Unit Price");
                Console.WriteLine("5.IMDB rating");
                Console.WriteLine();

                var propertySelection = Helpers.GetIntFromUser("What would you like to update: ");
                switch (propertySelection)
                {
                    case 1:
                        selectedMovie.Title = Helpers.GetStringFromUser("Enter new title: ");
                        break;
                    case 2:
                        selectedMovie.Duration = Helpers.GetIntFromUser("Enter new duration: ");
                        break;
                    case 3:
                        selectedMovie.Units = Helpers.GetIntFromUser("Enter how many units: ");
                        break;
                    case 4:
                        selectedMovie.UnitPrice = Helpers.GetIntFromUser("Enter new price: ");
                        break;
                    case 5:
                        selectedMovie.ImdbRating = Helpers.GetDoubleFromUser("Enter new IMDB rating: ");
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        return;
                }
                try
                {
                    db.SaveChanges();
                    Console.WriteLine();
                    Helpers.ReturnToUpdateCustomer("Movie updated successfully.", c);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the movie", ex);
                }
            }
        }
        internal static void DeleteMovies(Customer c)
        {
            Console.Clear();
            using (var db = new Context())
            {
                var movies = db.Movies.ToList();
                if (movies.Count == 0)
                {
                    Console.WriteLine("No movies found");
                    return;
                }
                for (int i = 0; i < movies.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {movies[i].Title}");
                }
                Console.WriteLine();
                var selection = Helpers.GetIntFromUser("Select a movie to delete: ");
                if (selection <= 0 || selection > movies.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }
                var selectedMovie = movies[selection - 1];
                db.Movies.Remove(selectedMovie);
                try
                {
                    db.SaveChanges();
                    Console.WriteLine($"The movie named {selectedMovie.Title} has deleted successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while deleting the movie: " + ex.Message);
                }
                Console.WriteLine();
                Helpers.ReturnToAdmin($"", c);
            }
        }
        internal static void OneCustomer(Customer c)
        {
            Console.Clear();
            Admin.DisplayCustomer(c);
            using (var db = new Context())
            {
                if (c != null)
                {
                    Console.WriteLine($"Username: {c.Username}");
                    Console.WriteLine($"Password: {c.Password}");
                    Console.WriteLine($"Firstname: {c.Firstname}");
                    Console.WriteLine($"Lastname: {c.Lastname}");
                    Console.WriteLine($"City: {c.City}");
                    Console.WriteLine($"Email: {c.Email}");
                    Console.WriteLine($"Phone: {c.Phone}");

                    var selectedCustomerMovies = db.CustomerMovies.Where(cm => cm.CustomerId == c.Id).ToList();
                    if (selectedCustomerMovies.Count > 0)
                    {
                        Console.Write("Movies: ");
                        foreach (var customerMovie in selectedCustomerMovies)
                        {
                            var movie = db.Movies.Where(m => m.Id == customerMovie.MovieId).FirstOrDefault();
                            Console.Write(movie.Title + ", ");
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("This customer has no movies.");
                    }
                    Console.WriteLine();
                    Helpers.ReturnToMain("Above is a detailed summary of your profile.", c);
                }
            }
        }

        internal static void DisplayCustomer(Customer c)
        {
            Console.Clear();
            string? displayUserName = ("User: " + c.Username + "\n");
            Console.WriteLine(displayUserName);
        }
    }
}
