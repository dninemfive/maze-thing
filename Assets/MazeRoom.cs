using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class MazeRoom : MonoBehaviour
    {
        public Directions doors = new Directions();
        public (int x, int y) position;
        public static Material White => MazeMaker.White;
        public static Material Black => MazeMaker.Black;
        public Dictionary<Direction, GameObject> doorToward = new Dictionary<Direction, GameObject>();
        public Dictionary<Direction, GameObject> roofToward = new Dictionary<Direction, GameObject>();
        public GameObject Camera;
        public Light Light;
        public bool HasDoneGeneration = false;
        #region doors
        public void OpenDoor(Direction d)
        {
            doors[d] = true;
            GameObject roof = roofToward[d];
            roof.GetComponent<Renderer>().material = Black;
            GameObject door = doorToward[d];
            door.SetActive(false);
        }
        public void CloseDoor(Direction d)
        {
            doors[d] = false;
            GameObject roof = roofToward[d];            
            roof.GetComponent<Renderer>().material = White;
            GameObject door = doorToward[d];
            door.SetActive(true);
        }
        public void OpenAllDoors()
        {
            foreach (Direction d in Directions.NESW) OpenDoor(d);
        }
        public void CloseAllDoors()
        {
            foreach (Direction d in Directions.NESW) CloseDoor(d);
        }
        public IEnumerable<Direction> DoorDirections
        {
            get
            {
                foreach(Direction d in Directions.NESW) if (doors[d]) yield return d;
            }
        } 
        public bool HasDoorFacing(Direction d)
        {
            return doors[d];
        }
        #endregion doors
        #region generation
        // Runs when the object is instantiated rather than when it's enabled. Using <c>Start</c> instead caused weird issues.
        void Awake()
        {
            // Setting all these in Unity directly seemed not to work. I'm probably missing something.
            Camera = transform.Find("Camera").gameObject;
            Camera.SetActive(false);
            Light = transform.Find("Point Light").gameObject.GetComponent<Light>();            
            foreach(Direction d in Directions.NESW)
            {
                GameObject door = transform.Find(d.Name() + " Door").gameObject;                
                GameObject roof = transform.Find(d.Name() + " Cardinal Roof").gameObject;
                doorToward[d] = door;
                roofToward[d] = roof;
            }
        }
        /// <summary>
        /// Finalizes room creation. Should always be run after instantiation. Needed to pass in the position of the room, as well as to handle the unique case of the first room.
        /// </summary>
        /// <param name="pos">This room's position in MazeRoom space.</param>
        /// <param name="initial">Whether this is the initial room and should therefore have all doors open and the camera enabled, or otherwise it should be generated normally.</param>
        public void PostStart((int x, int y) pos, bool initial = false)
        {
            transform.Find("Center Roof/CoordDisplay").gameObject.GetComponent<TextMeshPro>().SetText("(" + pos.x + "," + pos.y + ")");
            position = pos;
            Light.color = Color.HSVToRGB(Mathf.Clamp01(Mathf.Sin(position.x) + Mathf.Sin(position.y)), 1, 1);
            MazeMaker.MazeRooms[pos] = this;
            if (initial)
            {
                CameraManager.CurrentCamera = Camera;
                Camera.SetActive(true);
                OpenAllDoors();
            }
            else
            {
                CloseAllDoors();
                GenerateDoors();
            }
        }
        public void GenerateDoors()
        {
            int numDoors = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < numDoors; i++)
            {
                // note that this will produce duplicates sometimes. this is fine by me, since i want to bias toward fewer doors anyway.
                OpenDoor((Direction)UnityEngine.Random.Range(0, 4));
            }
            // add required doors
            MazeRoom temp;
            foreach (Direction d in Directions.NESW)
            {
                temp = MazeMaker.RoomFrom(position, d);
                if(temp != null && temp.HasDoorFacing(d.Opposite())) OpenDoor(d);
            }
        }
        public void GenerateNeighbors(int iterations = 0)
        {
            MazeMaker.Singleton.GenerateNeighbors(this, iterations);
        }
        #endregion generation
    }
}
