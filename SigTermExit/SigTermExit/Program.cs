using System.Reflection;
using System.Runtime.Loader;

namespace SigTermExit
{
    internal class Program
    {
        static volatile bool run = true;
        static void Main(string[] args)
        {
            Console.WriteLine("This is console");
            //CreateHostBuilder(args).Build().Run();

            AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()).Unloading += context =>
            {
                Console.WriteLine($"SIGTERM received, exiting program...{DateTime.Now}");
                run = false;
                Task.Delay(25 * 1000).Wait();
                Console.WriteLine($"Completed sigterm {DateTime.Now}");
            };

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            AssemblyLoadContext.Default.Unloading += Default_Unloading;

            //Console.CancelKeyPress += (x, ea) =>
            //{
            //    ea.Cancel = true;
            //    run = false;
            //    Console.WriteLine(ea.GetType());
            //    // Tell .NET to not terminate the process
            //    Console.WriteLine("Received SIGINT (Ctrl+C)");
            //    Task.Delay(5000).Wait();   
            //};

            while (run)
            {
                Task.Delay(3000).Wait();
                Console.WriteLine("Main loop");
            }
            Console.WriteLine($"Main loop Finished main loop {DateTime.Now}");
            Task.Delay(10000).Wait();
            Console.WriteLine($"Main loop Finished main loop 2 - {DateTime.Now}");
        }

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            Console.WriteLine($"assembly unloading {DateTime.Now}");
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine($"process exit event {DateTime.Now}");
        }
    }
    
}