using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scacchi_V2._0.Classi
{
    public class Cavallo : Pezzo
    {
        public Cavallo(int r, int c, string tipo, string colore, Image I, Grid g) : base(r, c, tipo, colore, I, g) { }

        public override void aggiornaMossePossibili()
        {
            mossePossibili.Clear();
            //Sinistra
            //Basso
            if (this.R + 1 <= 7 && this.C - 2 >= 0)
            {
                if(GetChildren(this.R + 1, this.C - 2) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C - 2},c");
                }
                else
                {
                    if(GetChildren(this.R + 1, this.C - 2).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C - 2},x");
                    }
                }
            }
            //Alto
            if (this.R - 1 >= 0 && this.C - 2 >= 0)
            {
                if (GetChildren(this.R - 1, this.C - 2) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C - 2},c");
                }
                else
                {
                    if (GetChildren(this.R - 1, this.C - 2).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C - 2},x");
                    }
                }
            }
            //Destra
            //Basso
            if (this.R + 1 <= 7 && this.C + 2 <= 7)
            {
                if (GetChildren(this.R + 1, this.C + 2) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C + 2},c");
                }
                else
                {
                    if (GetChildren(this.R + 1, this.C + 2).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C + 2},x");
                    }
                }
            }
            //Alto
            if (this.R - 1 >= 0 && this.C + 2 <= 7)
            {
                if (GetChildren(this.R - 1, this.C + 2) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C + 2},c");
                }
                else
                {
                    if (GetChildren(this.R - 1, this.C + 2).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C + 2},x");
                    }
                }
            }
            //Alto
            //Sinistra
            if (this.R - 2 >= 0 && this.C - 1 >= 0)
            {
                if (GetChildren(this.R -2, this.C - 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R -2}{this.C - 1},c");
                }
                else
                {
                    if (GetChildren(this.R - 2, this.C - 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R -2}{this.C - 1},x");
                    }
                }
            }
            //Destra
            if (this.R - 2 >= 0 && this.C + 1 <= 7)
            {
                if (GetChildren(this.R - 2, this.C + 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 2}{this.C + 1},c");
                }
                else
                {
                    if (GetChildren(this.R - 2, this.C + 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 2}{this.C + 1},x");
                    }
                }
            }
            //Basso
            //Sinistra
            if (this.R + 2 <= 7 && this.C - 1 >= 0)
            {
                if (GetChildren(this.R + 2, this.C - 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 2}{this.C - 1},c");
                }
                else
                {
                    if (GetChildren(this.R + 2, this.C - 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 2}{this.C - 1},x");
                    }
                }
            }
            //Destra
            if (this.R + 2 <= 7 && this.C + 1 <= 7)
            {
                if (GetChildren(this.R + 2, this.C + 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 2}{this.C + 1},c");
                }
                else
                {
                    if (GetChildren(this.R + 2, this.C + 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 2}{this.C + 1},x");
                    }
                }
            }
        }
    }
}
