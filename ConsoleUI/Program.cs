using GameEngine;

namespace ConsoleUI;

class Program()
{
    static void Main(string[] args)
    {
        Engine engine = new Engine();

        // Running the exe with no args loads the sample game.
        if (args.Length == 0)
        {
            args = new string[1];
            args[0] = "sample_game/sample_game.gge";
        }
        engine.GameLoop(args);
    }
}
