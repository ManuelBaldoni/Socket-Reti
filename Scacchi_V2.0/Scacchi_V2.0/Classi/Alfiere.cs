using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scacchi_V2._0.Classi
{
    public class Alfiere : Pezzo
    {
        public Alfiere(int r, int c, string tipo, string colore, Image I, Grid g) : base(r, c, tipo, colore, I, g) { }

        public override void aggiornaMossePossibili()
        {
            mossePossibili.Clear();
            alfiereMove();
        }
    }
}
