using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Kantar.DatabaseScriptRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("SqlServer");
            var scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts");

            if (!Directory.Exists(scriptsPath))
            {
                Console.WriteLine($"Scripts folder not found: {scriptsPath}");
                return;
            }

            var sqlFiles = Directory.GetFiles(scriptsPath, "*.sql", SearchOption.AllDirectories);
            Array.Sort(sqlFiles);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var file in sqlFiles)
                {
                    var script = File.ReadAllText(file);
                    Console.WriteLine($"Running script: {Path.GetFileName(file)}");

                    try
                    {
                        using (var cmd = new SqlCommand(script, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine($"Error running script: {Path.GetFileName(file)}");
                        Console.WriteLine(ex.ToString());
                        return;
                    }

                    Console.WriteLine("Done.");
                }
            }

            Console.WriteLine("All scripts executed.");
        }
    }
}
