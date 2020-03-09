using Microsoft.EntityFrameworkCore;
using ExamCSharp.Models;
namespace ExamCSharp.Contexts
{
    public class HomeContext: DbContext
    {
        public HomeContext (DbContextOptions options) : base(options){}
        public DbSet<User> Users {get;set;}

        public DbSet<Funthing> Funthings{get;set;}
        public DbSet<Response> Responses {get;set;}
    }
}