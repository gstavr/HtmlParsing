using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HtmlParserProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        async static Task MainAsync(string[] args)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://www.nzherald.co.nz/");
            var pageContents = await response.Content.ReadAsStringAsync();
            Console.WriteLine(pageContents);
            Console.ReadLine();
            Console.WriteLine("Hello World!");
        }
    }
}
