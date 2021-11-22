using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{    public class CameraManager : MonoBehaviour
    {
        public GameObject mainCamera;
        public GameObject CurrentCamera { get; private set; }
        public MazeRoom CurrentRoom
        {
            get
            {
                Vector3 playerCoords = Player.ActivePlayer.transform.position;
                (int x, int y) mazeRoomCoords = (Mathf.RoundToInt(playerCoords.x / 10), Mathf.RoundToInt(playerCoords.z / 10));
                return MazeMaker.RoomAt(mazeRoomCoords);
            }
        }
        public GameObject CameraForCurrentRoom => CurrentRoom.Camera;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ToggleCamera()
        {
            if(CurrentCamera == mainCamera)
            {
                SwitchCameraTo(CameraForCurrentRoom);
            } else
            {
                SwitchCameraTo(mainCamera);
            }
        }
        public void SwitchCameraTo(GameObject c)
        {
            if (CurrentCamera == c) return;
            CurrentCamera.SetActive(false);
            c.SetActive(true);
            CurrentCamera = c;
        }
    }
}

