using System;

namespace AtariBreakout
{
    internal class Menu
    {
        /// <summary>
        /// Delegate
        /// </summary>
        public delegate void MenuHandler(Item item);

        /// <summary>
        /// Evento que ocorre a hora que um item é escolhido
        /// </summary>
        public event MenuHandler ItemEscolhido;

        /// <summary>
        /// Enum contendo dodas opções do menu
        /// </summary>
        public enum Item
        {
            Jogar = 0,
            Sobre = 1,
            Sair = 2
        }

        /// <summary>
        /// Mostra o menuna tela
        /// </summary>
        public void Mostrar()
        {
            Desenhar();
            MenuEscolha();
        }

        /// <summary>
        /// Desenha o menu
        /// </summary>
        private void Desenhar()
        {

            Console.ResetColor();

            Desenhos.QuadroJanela();

            Console.SetCursorPosition((Console.WindowWidth / 2) - 7, 4);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Program.Titulo);
        }

        /// <summary>
        /// Menu
        /// </summary>
        private void MenuEscolha()
        {
            Item item = Item.Jogar;

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition((Console.WindowWidth / 2) - 4, 8);
            Console.Write(Item.Jogar.ToString());
            Console.SetCursorPosition((Console.WindowWidth / 2) - 4, 9);
            Console.Write(Item.Sobre.ToString());
            Console.SetCursorPosition((Console.WindowWidth / 2) - 4, 10);
            Console.Write(Item.Sair.ToString());

            while (true)
            {
                DesenhaCursor(item);
                ConsoleKeyInfo k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.Enter)
                {
                    if (ItemEscolhido != null)
                        ItemEscolhido(item);//dispara o evento
                    break;
                }

                if (k.Key == ConsoleKey.UpArrow && item != Item.Jogar)
                    item += (int)item == 0 ? 2 : -1;
                else if (k.Key == ConsoleKey.DownArrow && item != Item.Sair)
                    item += (int)item == 2 ? -2 : 1;
            }
        }

        /// <summary>
        /// Desenha o curçor de seleção
        /// </summary>
        /// <param name="apontado"></param>
        private void DesenhaCursor(Item apontado)
        {
            int width = (Console.WindowWidth / 2) - 6;

            Console.SetCursorPosition(width, 8 + ((int)apontado + 1 > 3 ? 0 : (int)apontado - 1));
            Console.Write("  ");
            Console.SetCursorPosition(width, 8 + ((int)apontado - 1 < 0 ? 1 : (int)apontado + 1));
            Console.Write("  ");

            Console.SetCursorPosition(width, 8 + (int)apontado);
            Console.Write("»");
        }
    }
}
