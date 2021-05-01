using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scacchi_V2._0.Classi
{
    public class Re : Pezzo
    {
        public Re(int r, int c, string tipo, string colore, Image I, Grid g) : base(r, c, tipo, colore, I, g) { }

        public override void aggiornaMossePossibili()
        {
            mossePossibili.Clear();
            //Alto sinistra
            if (this.R - 1 >= 0 && this.C - 1 >= 0)
            {
                if (GetChildren(this.R - 1, this.C - 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C - 1},c");
                }
                else
                {
                    if (GetChildren(this.R - 1, this.C - 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C - 1},x");
                    }
                }
            }
            //Alto
            if (this.R - 1 >= 0)
            {
                if (GetChildren(this.R - 1, this.C) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C},c");
                }
                else
                {
                    if (GetChildren(this.R - 1, this.C).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C},x");
                    }
                }
            }
            //Altro destra
            if (this.R - 1 >= 0 && this.C + 1 <= 7)
            {
                if (GetChildren(this.R - 1, this.C + 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C + 1},c");
                }
                else
                {
                    if (GetChildren(this.R - 1, this.C + 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R - 1}{this.C + 1},x");
                    }
                }
            }
            //Destra
            if (this.C + 1 <= 7)
            {
                if (GetChildren(this.R, this.C + 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{this.C + 1},c");
                }
                else
                {
                    if (GetChildren(this.R, this.C + 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{this.C + 1},x");
                    }
                }
            }
            //Basso destra
            if (this.R + 1 <= 7 && this.C + 1 <= 7)
            {
                if (GetChildren(this.R + 1, this.C + 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C + 1},c");
                }
                else
                {
                    if (GetChildren(this.R + 1, this.C + 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C + 1},x");
                    }
                }
            }
            //Basso
            if (this.R + 1 <= 7)
            {
                if (GetChildren(this.R + 1, this.C) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C},c");
                }
                else
                {
                    if (GetChildren(this.R + 1, this.C).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C},x");
                    }
                }
            }
            //Basso sinistra
            if (this.R + 1 <= 7 && this.C - 1 >= 0)
            {
                if (GetChildren(this.R + 1, this.C - 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C - 1},c");
                }
                else
                {
                    if (GetChildren(this.R + 1, this.C - 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R + 1}{this.C - 1},x");
                    }
                }
            }
            //Sinistra
            if (this.C - 1 >= 0)
            {
                if (GetChildren(this.R, this.C - 1) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{this.C - 1},c");
                }
                else
                {
                    if (GetChildren(this.R, this.C - 1).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{this.C - 1},x");
                    }
                }
            }

            if(!this.Mosso)
            {
                //Da aggiungere il fatto che non si può fare l'arrocco quando nelle mosse possibili c'è qualcosa
                //Arrocco sinistra
                bool ok = true;
                for (int i = this.C - 1; i > 0; i--)
                {
                    if(GetChildren(this.R, i) != null)
                    {
                        ok = false;
                    }
                }
                if(ok)
                {
                    if (GetChildren(this.R, this.C - 4) is Torre && !GetChildren(this.R, this.C - 4).Mosso)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{this.C - 2},o");
                    }
                }

                //Arrocco destra
                ok = true;
                for (int i = this.C + 1; i < 7; i++)
                {
                    if (GetChildren(this.R, i) != null)
                    {
                        ok = false;
                    }
                }
                if(ok)
                {
                    if (GetChildren(this.R, this.C + 3) is Torre && !GetChildren(this.R, this.C + 3).Mosso)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{this.C + 2},o");
                    }
                }
            }
        }
    }
}
