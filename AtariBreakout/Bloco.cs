
namespace AtariBreakout
{
    /// <summary>
    /// Classe que representa o bloco
    /// </summary>
    internal class Bloco
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int TamanhoEmX { get; set; }
        public int TamanhoEmY { get; set; }
        public bool Colidiu { get; set; }
    }
}
