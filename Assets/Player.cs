using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class Player : MonoBehaviour
    {
        public static GameObject ActivePlayer;
        // Start is called before the first frame update
        void Start()
        {
            ActivePlayer = gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
