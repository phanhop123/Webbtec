using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web_bao.Data;

namespace BusinessObject.Context
{
	public class ConnectDB : DbContext
	{
		public ConnectDB() { }
		public ConnectDB(DbContextOptions<ConnectDB> options) : base(options) { }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			IConfigurationRoot configuration = builder.Build();
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("Qlhs"));
		}
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Major> Majors { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<Course> Course { get; set; }
		public DbSet<Class_Admin> Class_Admins { get; set; }




	}
}
