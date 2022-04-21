using System;

namespace AtariBreakout
{
    class Program
    {
        /// <summary>
        /// Varivavel menu
        /// </summary>
        private static readonly Menu menu = new Menu();

        /// <summary>
        /// Constante titulo
        /// </summary>
        public const string Titulo = "Atari Breakout ";

        /// <summary>
        /// Breakout
        /// </summary>
        private static Breakout _breakout;

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Inicialização
            Console.Title = Titulo;
            Console.CursorVisible = false;
            _breakout = new Breakout();
            _breakout.FimJogo += breakout_FimJogo;

            //Menu ###########################
            menu.ItemEscolhido += menu_ItemEscolhido;

            menu.Mostrar();
        }

        /// <summary>
        /// Evento fim de jogo
        /// </summary>
        static void breakout_FimJogo()
        {
            menu.Mostrar();
        }

        /// <summary>
        /// Evento de item escolhido do menu
        /// </summary>
        /// <param name="item"></param>
        static void menu_ItemEscolhido(Menu.Item item)
        {
            switch (item)
            {
                case Menu.Item.Jogar: _breakout.Iniciar(); break;
                case Menu.Item.Sobre:
                    Desenhos.Sobre();
                    menu.Mostrar();
                    Console.ReadKey(true);
                    break;
                case Menu.Item.Sair: Environment.Exit(0); break;
            }
        }
    }
}
