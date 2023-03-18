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
            // Create an HttpListener instance with a prefix
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8080/");

                // Start listening for incoming requests
                listener.Start();
                Console.WriteLine("Listening...");

                // Loop until stopped
                bool running = true;
                while (running)
                {
                    try
                    {
                        // Get the context of the request
                        HttpListenerContext context = await listener.GetContextAsync();

                        // Check if it is a GET request
                        if (context.Request.HttpMethod == "GET")
                        {
                            // Write some response to the output stream
                            byte[] buffer = Encoding.UTF8.GetBytes("The current host name is: " + Dns.GetHostName());
                            context.Response.ContentLength64 = buffer.Length;
                            using (var stream = context.Response.OutputStream)
                            {
                                stream.Write(buffer, 0, buffer.Length);
                            }

                            // Close the output stream and send the response
                            context.Response.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle any exceptions that may occur
                        Console.WriteLine("Error: " + e.Message);
                        running = false; // Stop listening on error
                    }
                }

                // Stop listening when done
                listener.Stop();
            }
        }
    }
}