using System;
using System.Threading.Tasks;

namespace NotificationSvc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startup = new Startup();
            await startup.Run();
        }

    }
}
