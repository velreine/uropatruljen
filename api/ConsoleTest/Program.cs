using System;
using CommonData.Model.Static;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Nicky er sej, det her kører på en pie");

            var board = Board.SmartUroV1;

            Console.WriteLine(board.ToString());
        }
    }
}
