
namespace AiClient
{
    public class Program
    {
        public static Engine Engine { get; private set; }

        static void Main(string[] args)
        {
            Engine = new Engine();
            Engine.Run();
        }
    }
}
