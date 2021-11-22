using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class Player : MonoBehaviour
    {
        public static Player ActivePlayer;
        public CharacterController controller;
        public GameObject Light = null;
        public (int x, int y) MazeRoomCoords => (Mathf.RoundToInt(transform.position.x / 10), Mathf.RoundToInt(transform.position.z / 10));
        private (int x, int y) prevMazeRoomCoords = (0, 0);
        public MazeRoom CurrentRoom => MazeMaker.RoomAt(MazeRoomCoords);
        public float speed = 10;
        // Start is called before the first frame update
        void Start()
        {
            ActivePlayer = this;
        }

        // Update is called once per frame
        void Update()
        {            
            float v = Input.GetAxis("Vertical"),
                  h = Input.GetAxis("Horizontal");
            Vector3 velocity = Vector3.zero;
            velocity += transform.forward * v * speed;
            velocity += transform.right * h * speed;
            controller.SimpleMove(velocity);
            float rot = 0;
            if (Input.GetKey(KeyCode.Q)) rot -= 1;
            if (Input.GetKey(KeyCode.E)) rot += 1;
            Vector3 angle = transform.eulerAngles;
            angle.y += rot * Time.deltaTime * 60;
            transform.eulerAngles = angle;
            if (MazeRoomCoords != prevMazeRoomCoords) OnNewRoom();
            prevMazeRoomCoords = MazeRoomCoords;
        }
        public void OnNewRoom()
        {
            if(Light != null)
            {
                if (MazeRoomCoords == (0, 0)) Light.SetActive(false);
                else Light.SetActive(true);
            }
            MazeRoom curRoom = MazeMaker.RoomAt(MazeRoomCoords);
            if(!curRoom.HasDoneGeneration)
            {
                curRoom.GenerateNeighbors();
                curRoom.HasDoneGeneration = true;
            }
            CameraManager.SwitchToNewRoomCamera();
        }
    }
}
