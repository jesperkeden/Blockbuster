using System;
using Blockbuster.Data;
using Blockbuster.Models;

namespace Blockbuster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Step 1 - Fill up the database with Genres, Movies and an admin.
            //Helpers.ImportToDatabase();

            //Step 2 - Comment out the line above, and decomment the line below.
            Menus.RunShow();

            //Step 3 - Login as admin to create movies, update customers, watch queries and more..
            //username = admin
            //password = admin

            //Step 4 - Logout and create a new customer.
            //Enjoy and start collecting movies.

            //FORMATERA så att det blir snyggt med priset på filmer i Showmoviedisplayw/e
        }
    }
}