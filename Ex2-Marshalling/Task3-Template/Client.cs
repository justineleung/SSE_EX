using System;
using System.Threading.Tasks;

namespace Task3
{
    public class Client
    {
        private readonly Middleware _middleware;

        public Client(Middleware middleware)
        {
            _middleware = middleware;
        }

        public async Task Print(string address, double d)
        {
            Console.WriteLine("Client: Received response: " + await _middleware.SendObjectTo(address, d));
        }

        public async Task Print(string address, string line)
        {
            Console.WriteLine("Client: Received response: " + await _middleware.SendObjectTo(address, line));
        }

        public async Task Print(string address, String[] page)
        {
            Console.WriteLine("Client: Received response: " + await _middleware.SendObjectTo(address, page));
        }


        public async Task<string> concat(string arg1, string arg2, string arg3)
        {
            // --------------------- Implement ---------------------
            // TODO: Call server
            string myAddress = "I dont know";
            string[] myArgs = new string[3] {arg1, arg2, arg3 };
            return await _middleware.CallFunction(myAddress, "concat", myArgs);
            // --------------------- /Implement -------------------

            return "";
        }

        public async Task<string> substring(string s, string startingPosition)
        {
            // --------------------- Implement ---------------------
            // TODO: Call server
            string myAddress = "I dont know";
            string[] myArgs = new string[2] { s, startingPosition };
            return await _middleware.CallFunction(myAddress, "substring", myArgs);
            // --------------------- /Implement -------------------

            return "";
        }
    }
}
