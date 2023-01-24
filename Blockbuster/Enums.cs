using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockbuster
{
    internal class Enums
    {
        public enum MenuList
        {
            Sign_In = 1,
            Create_Customer = 2,
            About = 3,
            Exit = 4,
        }
        public enum CustomerMainList
        {
            Browse_Movies = 1,
            Admin = 2,
            Go_back_to_previous = 3,
        }
        public enum AdminMainList
        {
            Create_Movie = 1,
            Delete_Movie = 2,
            Update_Customer = 3,
            Delete_Customer = 4,
        }
        public enum AdminUpdateList
        {
            Username = 1,
            Password = 2,
            Firstname = 3,
            Lastname = 4,
            Email = 5,
            Go_back_to_previous = 6,
        }
        public enum BrowseGenreList
        {
            Action = 1,
            Adventure = 2,
            Comedy = 3,
            Go_back_to_previous = 4,
        }
        public enum GenreMovieList
        {
            Username = 1,
            Password = 2,
            Firstname = 3,
            Lastname = 4,
            Email = 5,
            Go_back_to_previous = 6,
        }
    }
}
