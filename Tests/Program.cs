using System;

namespace Tests
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            try
            {
                Console.WriteLine ("Running Tests...");
                BlocksColumnTests.Test ();
                Console.WriteLine ("OK");
            }
            catch (Exception e) {
                Console.WriteLine ($"Fail {e.Message}");
            }
            Console.ReadKey ();
        }
    }
}
