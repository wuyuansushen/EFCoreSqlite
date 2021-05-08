using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace EFCoreSqlite
{
    class Program
    {
        public class BloggingContext:DbContext
        {
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }

            protected override void OnConfiguring([NotNull]DbContextOptionsBuilder optionsBuilder)
            {
                //Warning: Build binary file isn't exported in current directory.
                optionsBuilder.UseSqlite(@"Data Source=./../../../blogging.db");
            }
        }

        public class Blog
        {
            public int BlogID { get; set; }
            public string Url { get; set; }
            public List<Post> Posts { get; } = new List<Post>();
        }

        public class Post
        {
            public int PostID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public int BlogID { get; set; }
        }
        static void Main(string[] args)
        {
            using var db = new BloggingContext();

            Console.WriteLine("Inserting...");
            db.Add(new Blog { Url = "https://github.com" });
            db.SaveChanges();

            Console.WriteLine("Querying first entity...");
            var blog = db.Blogs.OrderBy(b => b.BlogID).First();
            blog.Url = @"https://cn.bing.com/";
            blog.Posts.Add(new Post { Title = "First Post", Content = @"First Post information." });
            db.SaveChanges();
            db.Remove(blog);
            db.SaveChanges();
        }
    }
}
