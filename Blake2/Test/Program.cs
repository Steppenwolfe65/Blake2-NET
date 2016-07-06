#region Directives
using System;
#endregion

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUtils.SizeConsole(80, 60);
            ConsoleUtils.CenterConsole();
            Console.Title = "CEX Test Suite";
            Console.BufferHeight = 600;
            
            Console.WriteLine("**********************************************");
            Console.WriteLine("* CEX Version 1.5                            *");
            Console.WriteLine("*                                            *");
            Console.WriteLine("* Release:   v1.5.6                          *");
            Console.WriteLine("* Date:      May 28, 2016                    *");
            Console.WriteLine("* Contact:   develop@vtdev.com               *");
            Console.WriteLine("**********************************************");
            Console.WriteLine("");

            Console.WriteLine("******TESTING BLOCK CIPHERS******");

            Console.WriteLine("******TESTING MESSAGE DIGESTS******");
            RunTest(new Blake2Test());
            Console.WriteLine("");

            Console.WriteLine("Completed! Press any key to close..");
            Console.ReadKey();
        }

        private static void RunTest(ITest Test)
        {
            try
            {
                Test.Progress += new EventHandler<TestEventArgs>(OnTestProgress);
                Console.WriteLine(Test.Description);
                Console.WriteLine(Test.Run());
                Console.WriteLine();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("An error has occured!");
                Console.WriteLine(Ex.Message);
                Console.WriteLine("");
                Console.WriteLine("Continue Testing? Press 'Y' to continue, all other keys abort..");
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (!keyInfo.Key.Equals(ConsoleKey.Y))
                    Environment.Exit(0);
            }
            finally
            {
                Test.Progress -= OnTestProgress;
            }
        }

        private static void OnTestProgress(object sender, TestEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
