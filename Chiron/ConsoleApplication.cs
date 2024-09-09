
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Chiron
{
    class ConsoleApplication
    {
        private IConfiguration Configuration;

        public ConsoleApplication(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public void Run()
        {
            PageProcessor pageProcessor = new PageProcessor(Configuration);
            WordProcessor processor = new WordProcessor(pageProcessor, Configuration);

            PrintCommands();

            try
            {
                while (true)
                {
                    Console.Write("Enter command, then press ENTER: ");
                    string decision = Console.ReadLine();
                    switch (decision.ToLower())
                    {
                        case "1":
                            processor.ProcessLexique();
                            break;
                        case "2":
                            processor.ProcessPages();
                            break;
                        case "3":
                            processor.ShortenLexicon();
                            break;
                        case "4":
                            pageProcessor.LoadAvailablePhonemes();
                            break;
                        case "5":
                            pageProcessor.WritePages();
                            break;
                        case "6":
                            pageProcessor.WriteWritingPages();
                            break;
                        case "7":
                            pageProcessor.WriteSeyesPages();
                            break;
                        case "help":
                            PrintCommands();
                            break;
                        case "exit":
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid command. Enter 'help' to show a list of commands.");
                            Console.ResetColor();
                            break;
                    }

                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex}");
                Console.ResetColor();
            }
            Console.ReadLine();
        }

        private void PrintCommands()
        {
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Command  Description");
            Console.WriteLine("====================");
            Console.WriteLine("[1]      Process Lexique");
            Console.WriteLine("[2]      Process Pages");
            Console.WriteLine("[3]      Shorten Lexicon");
            Console.WriteLine("[4]      Load available phonèmes");
            Console.WriteLine("[5]      Print Phonèmes");
            Console.WriteLine("[6]      Print Writing Page");
            Console.WriteLine("[7]      Print Seyes Page");
            // Console.WriteLine("[6]      Create users (bulk import)");
            // Console.WriteLine("[7]      Create user with custom attributes and show result");
            // Console.WriteLine("[8]      Get all users (one page) with custom attributes");
            // Console.WriteLine("[9]      Get the number of useres in the directory");
            // Console.WriteLine("[10]      Update custom attributes");
            Console.WriteLine("[help]   Show available commands");
            Console.WriteLine("[exit]   Exit the program");
            Console.WriteLine("-------------------------");
        }
    }
}
