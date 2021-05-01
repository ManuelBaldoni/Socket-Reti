using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scacchi_V2._0.Classi
{
    public class Pedone : Pezzo
    {
        public Pedone(int r, int c, string tipo, string colore, Image I, Grid g) : base(r, c, tipo, colore, I, g) { }

        public override void aggiornaMossePossibili()
        {
            mossePossibili.Clear();
            if(this.Colore == "b")
            {
                //Mosse base
                if(GetChildren(this.R - 1, this.C) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C},c");
                    if (!this.Mosso && GetChildren(this.R - 2, this.C) == null)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 2}{this.C},c");
                    }
                }
                //Mosse mangiabili
                if(this.C != 0)
                {
                    if(GetChildren(this.R - 1, this.C - 1) != null && GetChildren(this.R - 1, this.C - 1).Colore == "n")
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C - 1},x");
                    }
                }
                if (this.C != 7)
                {
                    if (GetChildren(this.R - 1, this.C + 1) != null && GetChildren(this.R - 1, this.C + 1).Colore == "n")
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C + 1},x");
                    }
                }
            }
            else
            {
                //Mosse base
                if (GetChildren(this.R + 1, this.C) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C},c");
                    if (!this.Mosso && GetChildren(this.R + 2, this.C) == null)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 2}{this.C},c");
                    }
                }
                //Mosse mangiabili
                if (this.C != 0)
                {
                    if (GetChildren(this.R + 1, this.C - 1) != null && GetChildren(this.R + 1, this.C - 1).Colore == "b")
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C - 1},x");
                    }
                }
                if (this.C != 7)
                {
                    if (GetChildren(this.R + 1, this.C + 1) != null && GetChildren(this.R + 1, this.C + 1).Colore == "b")
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C + 1},x");
                    }
                }
            }
        }

    }
}
