using System;
using System.Linq;
using System.Threading;

namespace AtariBreakout
{
    internal class Breakout
    {
        /// <summary>
        /// Pontos do jogador
        /// </summary>
        public int Pontos { get; set; }

        #region Váriaveis internas

        /// <summary>
        /// Matriz de blocos
        /// </summary>
        private Bloco[][] _blocos;

        /// <summary>
        /// Barra a ser controlada pelo usuário
        /// </summary>
        private Bloco _barraInteracao;

        /// <summary>
        /// Bola
        /// </summary>
        private Bola _bolaInteracao;

        /// <summary>
        /// Contador para apontar a linha correta na matriz
        /// </summary>
        private int _linhaDesenhada;

        /// <summary>
        /// thread responsavel por mover a bola
        /// </summary>
        private Thread _threadBola;

        /// <summary>
        /// modificador do angulo x
        /// </summary>
        private int _modificadorAnguloX;

        /// <summary>
        /// modificador o angulo y
        /// </summary>
        private int _modificadorAnguloY;

        /// <summary>
        /// modificador de velocidade
        /// </summary>
        private int _modificadorDeVelocidade;

        /// <summary>
        /// Quantidade de vidas
        /// </summary>
        private int _vidas;
        #endregion

        #region Eventos
        /// <summary>
        /// Delegate
        /// </summary>
        public delegate void BreakoutHandler();

        /// <summary>
        /// Evento que ocorre quando o jogo chega ao fim
        /// </summary>
        public event BreakoutHandler FimJogo;

        /// <summary>
        /// Evento que ocorre quando a bola toca o plano inferiordo jogo
        /// </summary>
        public event BreakoutHandler PerdaVida;
        #endregion

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public Breakout()
        {
            Pontos = 0;
            _blocos = new Bloco[3][];
            _linhaDesenhada = 0;

            _barraInteracao = new Bloco
                {
                    TamanhoEmX = 14,
                    TamanhoEmY = 1,
                    X = Console.WindowWidth / 2 - 7,
                    Y = Console.WindowHeight - 2
                };

            _vidas = 3;
            PerdaVida += Breakout_PerdaVida;
        }

        /// <summary>
        /// Inicia o jogo
        /// </summary>
        public void Iniciar()
        {
            Desenhar();

            IniciarMovimentacaoBola();

            CapturaComandos();
        }

        #region Métodos privados

        /// <summary>
        /// MEtodo evento para perca de vida
        /// </summary>
        private void Breakout_PerdaVida()
        {
            _vidas--;
            if (_vidas == 0)
                FimJogo();
        }

        /// <summary>
        /// Inicia a movimentação da bolinha
        /// </summary>
        private void IniciarMovimentacaoBola()
        {
            _bolaInteracao = new Bola
                {
                    X = Console.WindowWidth / 2,
                    Y = Console.WindowHeight - 3,
                };

            _modificadorAnguloX = -1; //randon de-3 a 3
            _modificadorAnguloY = -1;
            _modificadorDeVelocidade = 0;

            Desenhos.Bola(_bolaInteracao, ConsoleColor.White);

            _threadBola = new Thread(MovimentarBola);
            _threadBola.Start();
        }

        /// <summary>
        /// Providencia amovimentação da bola
        /// </summary>
        private void MovimentarBola()
        {
            while (true)
            {
                Thread.Sleep(250 - _modificadorDeVelocidade);

                Desenhos.Bola(_bolaInteracao);


                _bolaInteracao.Y += _modificadorAnguloY;
                _bolaInteracao.X += _modificadorAnguloX;

                if (_bolaInteracao.Y < 1)
                    _bolaInteracao.Y = 1;
                else if (_bolaInteracao.Y > Console.WindowHeight - 1)
                    _bolaInteracao.Y = Console.WindowHeight - 1;

                if (_bolaInteracao.X < 1)
                    _bolaInteracao.X = 1;
                else if (_bolaInteracao.X >= Console.WindowWidth - 1)
                    _bolaInteracao.X = Console.WindowWidth - 2;

                VerificarColisao();

                Desenhos.Bola(_bolaInteracao, ConsoleColor.White);

                Desenhos.Placar(Pontos);

                // Verifica se todos o blocos já foram acertados e dispara o evento de
                // fim de jogo
                if (!_blocos.Any(blocoArray => blocoArray.Any(bloco => !bloco.Colidiu)))
                {
                    FimJogo();
                    Thread.CurrentThread.Abort();
                }
            }
        }

        /// <summary>
        /// Verifica se a bola colidiu com algum objeto
        /// </summary>
        private void VerificarColisao()
        {
            //colisao do bloco principal
            if ((_bolaInteracao.Y >= Console.WindowHeight - 3)
                && (_bolaInteracao.X >= _barraInteracao.X && _bolaInteracao.X <= _barraInteracao.X + _barraInteracao.TamanhoEmX))
            {
                int mod = 0;
                if (_bolaInteracao.X < (_barraInteracao.X + 3))
                    mod = 3;
                else if (_bolaInteracao.X < (_barraInteracao.X + 6))
                    mod = 2;
                else if (_bolaInteracao.X <= (_barraInteracao.X + 7))
                    mod = 1;
                else if (_bolaInteracao.X <= (_barraInteracao.X + 8))
                    mod = -1;
                else if (_bolaInteracao.X < (_barraInteracao.X + 11))
                    mod = -2;
                else if (_bolaInteracao.X < (_barraInteracao.X + 14))
                    mod = -3;

                _modificadorAnguloX = mod;

                _modificadorAnguloY *= -1;
                _modificadorAnguloX *= -1;

                return;
            }

            //colisão do fundo
            if (_bolaInteracao.Y == Console.WindowHeight - 1)
            {
                Pontos -= 30;
                IniciarMovimentacaoBola();
                PerdaVida();
                Thread.CurrentThread.Abort();
            }

            //colisão borda topo
            if (_bolaInteracao.Y == 1)
            {
                _modificadorAnguloY *= -1;
                return;
            }

            //colisao das laterais
            if (_bolaInteracao.X <= 1
                || _bolaInteracao.X >= Console.WindowWidth - 2)
            {
                _modificadorAnguloX *= -1;
                return;
            }



            //colisao dos blocos
            foreach (Bloco[] item in _blocos)
            {
                foreach (Bloco bloco in item)
                {
                    if ((_bolaInteracao.X >= bloco.X && _bolaInteracao.X <= (bloco.X + bloco.TamanhoEmX))
                        && (_bolaInteracao.Y >= bloco.Y && _bolaInteracao.Y <= (bloco.Y + bloco.TamanhoEmY))
                        && !bloco.Colidiu)
                    {

                        Desenhos.Bola(_bolaInteracao, ConsoleColor.White);

                        bloco.Colidiu = true;

                        Desenhos.Bloco(bloco);

                        Pontos += 10;

                        if (_modificadorDeVelocidade < 50)
                            _modificadorDeVelocidade += 5;

                        //verifica se a colisao é lateral
                        if (_bolaInteracao.Y >= bloco.Y
                            && _bolaInteracao.Y <= (bloco.Y + (bloco.TamanhoEmY > 1 ? bloco.TamanhoEmY : 0)))
                            _modificadorAnguloX *= -1;
                        else
                            _modificadorAnguloY *= -1;

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Comandos para mover a barra
        /// </summary>
        private void CapturaComandos()
        {
            while (true)
            {
                ConsoleKey tecla = Console.ReadKey(true).Key;

                if (tecla == ConsoleKey.LeftArrow || tecla == ConsoleKey.RightArrow)
                {
                    Desenhos.Bloco(_barraInteracao);

                    int movimentacao = 0;

                    if (tecla == ConsoleKey.LeftArrow && _barraInteracao.X > 1)
                        movimentacao = -1;
                    if (tecla == ConsoleKey.RightArrow && _barraInteracao.X + 1 < Console.WindowWidth - _barraInteracao.TamanhoEmX)
                        movimentacao = 1;
                    _barraInteracao.X += movimentacao;
                    Desenhos.Bloco(_barraInteracao, ConsoleColor.Gray);
                }
            }
        }

        /// <summary>
        /// Desenha o Breakout
        /// </summary>
        private void Desenhar()
        {
            Console.Clear();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Desenhos.QuadroJanela();

            DesenharBlocos(2, 2, 3, 1, ConsoleColor.Red);

            DesenharBlocos(4, 4, 5, 1, ConsoleColor.Yellow);

            DesenharBlocos(6, 2, 3, 1, ConsoleColor.Blue);

            Desenhos.Bloco(_barraInteracao, ConsoleColor.Gray);
        }

        /// <summary>
        /// Instancia e desenha os blocos do jogo na tela
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="coluna"></param>
        /// <param name="tamanhoX"></param>
        /// <param name="tamanhoY"></param>
        /// <param name="cor">cor do objeto</param>
        private void DesenharBlocos(int linha, int coluna, int tamanhoX, int tamanhoY, ConsoleColor cor = ConsoleColor.Black)
        {
            int quantidadeBlocos = 0;

            switch (tamanhoX)
            {
                case 5: quantidadeBlocos = 12; break;
                case 3: quantidadeBlocos = 19; break;
            }

            //blocos da linha
            _blocos[_linhaDesenhada] = new Bloco[quantidadeBlocos];

            for (int i = 0; i < _blocos[_linhaDesenhada].Length; i++)
            {
                _blocos[_linhaDesenhada][i] = new Bloco
                {
                    TamanhoEmX = tamanhoX,
                    TamanhoEmY = tamanhoY,
                    Y = linha,
                    X = coluna
                };

                coluna += tamanhoX + 1;

                Desenhos.Bloco(_blocos[_linhaDesenhada][i], cor);
            }

            _linhaDesenhada++;
        }
        #endregion
    }
}
