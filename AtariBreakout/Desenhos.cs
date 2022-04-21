using System;

namespace AtariBreakout
{
    internal class Desenhos
    {
        private static object mutex = new object();

        /// <summary>
        /// Desenha o quadro de contorno do console
        /// </summary>
        public static void QuadroJanela()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.SetCursorPosition(Console.WindowWidth - 1, 0);
            Console.Write("╗");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("╚");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("╝");

            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");

                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("═");

                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("═");
            }
        }

        /// <summary>
        /// Deseha um bloco na tela
        /// </summary>
        /// <param name="bloco"></param>
        /// <param name="cor"></param>
        public static void Bloco(Bloco bloco, ConsoleColor cor = ConsoleColor.Black)
        {
            lock (mutex)
            {
                Console.ForegroundColor = cor;
                for (int i = 0; i < bloco.TamanhoEmY; i++)
                {
                    for (int k = 0; k < bloco.TamanhoEmX; k++)
                    {
                        Console.SetCursorPosition(bloco.X + k, bloco.Y + i);
                        Console.Write("█");
                    }
                }
            }
        }

        /// <summary>
        /// Desenha uma bola na tela
        /// </summary>
        /// <param name="bola"></param>
        /// <param name="cor"></param>
        public static void Bola(Bola bola, ConsoleColor cor = ConsoleColor.Black)
        {
            lock (mutex)
            {
                Console.ForegroundColor = cor;
                Console.SetCursorPosition(bola.X, bola.Y);
                Console.Write("■");
            }
        }

        /// <summary>
        /// Imprime o placar na tela
        /// </summary>
        /// <param name="pontos"></param>
        public static void Placar(int pontos)
        {
            lock (mutex)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(2, 0);
                Console.Write("Placar: {0} ",pontos);
            }
        }

        /// <summary>
        /// Desenha o sbre na tela
        /// </summary>
        public static void Sobre()
        {
            Console.Clear();


            Console.SetCursorPosition((Console.WindowWidth / 2) - 7, 4);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Program.Titulo);

            Console.ForegroundColor = ConsoleColor.Blue;
            QuadroJanela();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.SetCursorPosition(19, 8);
            Console.WriteLine("###########################################");
            Console.SetCursorPosition(19, 9);
            Console.WriteLine("###########################################");
            Console.SetCursorPosition(19, 10);
            Console.WriteLine("#####                                 #####");
            Console.SetCursorPosition(19, 11);
            Console.WriteLine("#####                                 #####");
            Console.SetCursorPosition(19, 12);
            Console.WriteLine("#####                                 #####");
            Console.SetCursorPosition(19, 13);
            Console.WriteLine("###########################################");
            Console.SetCursorPosition(19, 14);
            Console.WriteLine("###########################################");
            Console.SetCursorPosition(19, 15);

            Console.SetCursorPosition(29, 11);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Criado por Magi");
        }
    }
}
