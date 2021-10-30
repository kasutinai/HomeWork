using System;

namespace SOLID
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GuessTheNumberGame(new NumberGenerator(), new ConsoleInterface());

            game.Play();
        }
    }
}
