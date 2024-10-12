using GameEngine;

namespace ConsoleUI;

class Program()
{
    static void Main(string[] args)
    {
        Engine engine = new Engine();

        args = new string[1];
        args[0] = "c:\\temp\\test_game_database.gge";
        engine.GameLoop(args);
    }
}
