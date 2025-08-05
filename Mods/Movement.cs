using easyInputs;
using Il2CppSystem.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public static void TriggerFly()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) || EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaTagger.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * 10f * Time.deltaTime; // kinda slow but like just make it faster
            }
        }
        public static void SpeedBosst()
        {
            GorillaLocomotion.Player.Instance.maxJumpSpeed = 7.5f;
            GorillaLocomotion.Player.Instance.jumpMultiplier = 5f;
        }
    }
}

