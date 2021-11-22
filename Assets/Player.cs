using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// The instance of the unique Player.
        /// </summary>
        public static Player ActivePlayer;
        public CharacterController controller;
        /// <summary>
        /// The <c>GameObject</c> holding  a <c>Light</c> component attached to the player. Currently unused, but could easily have such a component plugged in.
        /// </summary>
        public GameObject Light = null;
        /// <summary>
        /// The player's current coordinates in maze space. See <c>MazeRoom.position</c> for more info.
        /// </summary>
        public (int x, int y) MazeRoomCoords => (Mathf.RoundToInt(transform.position.x / 10), Mathf.RoundToInt(transform.position.z / 10));
        /// <summary>
        /// The player's maze space coordinates on the previous <c>Update()</c> call. Used to determine when the player enters a new room.
        /// </summary>
        private (int x, int y) prevMazeRoomCoords = (0, 0);
        /// <summary>
        /// The <c>MazeRoom</c> the player is currently in.
        /// </summary>
        public MazeRoom CurrentRoom => MazeMaker.RoomAt(MazeRoomCoords);
        /// <summary>
        /// The speed at which the player can move in each cardinal direction at a given time. Because I treat each separately, the player can move sqrt(2) times this amount diagonally.
        /// </summary>
        public float speed = 10;
        void Start()
        {
            ActivePlayer = this;
        }
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
