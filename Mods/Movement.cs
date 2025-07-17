using Il2CppSystem.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiTemp.Mods
{
    internal class Movement
    {
        public static void LongArms(bool on)//couldve made enable/disable func but idc 
        {
            if (on)
            {
                GorillaTagger.Instance.transform.localScale = new UnityEngine.Vector3(1.25f, 1.25f, 1.25f);
            }
            else
            {
                GorillaTagger.Instance.transform.localScale = UnityEngine.Vector3.one;

            }
        }
        }
    }
