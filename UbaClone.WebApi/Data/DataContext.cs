using Microsoft.EntityFrameworkCore;

namespace UbaClone.WebApi.Data
{
    public class DataContext: DbContext
    {
       
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        string database = "UbaClone.Db";
        //        string dir = Environment.CurrentDirectory;
        //        string path = String.Empty;

        //        if (dir.EndsWith("net8.0"))
        //        {
        //            // In the <project>\bin\<Debug|Release>\net8.0 directory.
        //            path = Path.Combine("..", "..", "..", "..", database);
        //        }
        //        else
        //        {
        //            //In the <project> directory
        //            path = Path.Combine("..", database);
        //        }

        //        path = Path.GetFullPath(path);

        //        if (!File.Exists(path)) 
        //        {
        //            throw new FileNotFoundException(message: $"{path} Not found.", fileName: path);
        //        }

        //        optionsBuilder.UseSqlite($"Data Source={path}");

        //    }
        //}
        public DbSet<Models.UbaClone> ubaClones { get; set; }

    }
}



