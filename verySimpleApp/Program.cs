using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpListenerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8080/");

                listener.Start();
                Console.WriteLine("Listening...");

                bool running = true;
                while (running)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();

                        if (context.Request.HttpMethod == "GET")
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes("The current host name is: " + Dns.GetHostName());
                            context.Response.ContentLength64 = buffer.Length;
                            using (var stream = context.Response.OutputStream)
                            {
                                stream.Write(buffer, 0, buffer.Length);
                            }

                            context.Response.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        running = false;
                    }
                }

                listener.Stop();
            }
        }
    }
}
