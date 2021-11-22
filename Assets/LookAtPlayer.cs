using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class LookAtPlayer : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Player.ActivePlayer.transform);
        }
    }
}