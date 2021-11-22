﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{    public class CameraManager : MonoBehaviour
    {
        public GameObject mainCamera;
        public static GameObject MainCamera => Singleton.mainCamera;
        public static GameObject CurrentCamera { get; set; }
        public static GameObject CameraForCurrentRoom => Player.ActivePlayer.CurrentRoom.Camera;
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

