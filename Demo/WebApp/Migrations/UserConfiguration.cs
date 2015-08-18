using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Migrations
{
    internal sealed class UserConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public UserConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }     
    }
}