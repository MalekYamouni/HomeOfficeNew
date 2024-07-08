using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore;

namespace HomeOffice
{
	/// <summary>
	/// Contains the main method of this application.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The main method of the application.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}
		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder<Startup>(args)
					 .UseContentRoot(Directory.GetCurrentDirectory())
					 .UseUrls("http://localhost:5001") // Setzt die URL auf HTTP
					 .Build();

#if DEBUG
		private const bool DebugMode = true;
#else
		private const bool DebugMode = false;
#endif
	}
}

