using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN.UtilityScripts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GuiTemp.Classes
{
    internal class RigManager // heavily inspired from iidk
    {
        public static VRRig GetRigFromPhotonPlayer(Photon.Realtime.Player player)
        {
            if (player != null)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig.photonView.Owner == player)
                    {
                        return vrrig;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static Player GetPlayerFromRig(VRRig vrrig)
        {
            if (vrrig != null)
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player != null)
                    {
                        if (vrrig.GetComponent<PhotonView>().Owner == player)
                        {
                            return player;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static bool IsRigTagged(VRRig vrrig)
        {
            if (vrrig != null)
            {
                foreach (GorillaTagManager GTM in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                {
                    if (GTM.currentInfected.Contains(GetPlayerFromRig(vrrig)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public static bool IsPlayerTagged(Player player)
        {
            if (player != null)
            {
                foreach (GorillaTagManager GTM in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                {
                    if (GTM.currentInfected.Contains(player))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public static VRRig GetAllRigs()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                return vrrig;
            }
            return null;
        }
        public static VRRig GetRigFromUserID(string UserID)
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig.photonView.Owner.UserId == UserID)
                {
                    return vrrig;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public static string GetIdFromRig(VRRig vrrig)
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.UserId == vrrig.photonView.Owner.UserId)
                {
                    return player.UserId;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}