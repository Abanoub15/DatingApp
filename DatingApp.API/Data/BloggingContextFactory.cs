using System.Diagnostics;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class BloggingContextFactory : IDesignTimeDbContextFactory<DataContext>
    {

    DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server =(local)\\sqlexpress; Database=AngularDb; data source=.; Trusted_Connection=True; MultipleActiveResultSets=True; App=EntityFramework&quot; integrated security=True;");
            return new DataContext(optionsBuilder.Options);
    }
}