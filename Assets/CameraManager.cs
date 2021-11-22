using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// The top-down camera used for the minimap. <c>mainCamera</c> is set in the Unity scene; <c>MainCamera</c> is an alias so as not to need singleton references.
        /// </summary>
        public GameObject mainCamera;
        public static GameObject MainCamera => Singleton.mainCamera;
        /// <summary>
        /// The current non-minimap camera, i.e. the one for the room the player is currently in. When transitioning between rooms, will contain the camera for the previous room before switching.
        /// </summary>
        public static GameObject CurrentCamera;
        /// <summary>
        /// The camera for the room the player is in. This will usually match <c>CurrentCamera</c>, except for the instant the player travels between rooms.
        /// </summary>
        public static GameObject CameraForCurrentRoom => Player.ActivePlayer.CurrentRoom.Camera;
        /// <summary>
        /// The unique instance of the CameraManager, which must not be static because it's a <c>MonoBehaviour</c>.
        /// </summary>
        public static CameraManager Singleton;
        void Start()
        {
            if(Singleton != null)
            {
                Destroy(this);
                return;
            }
            Singleton = this;
        }
        public static void SwitchCameraTo(GameObject c)
        {
            if (CurrentCamera == c) return;
            CurrentCamera.SetActive(false);
            c.SetActive(true);
            CurrentCamera = c;
        }
        public static void SwitchToNewRoomCamera()
        {
            SwitchCameraTo(CameraForCurrentRoom);
        }
    }
}

