using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Debug.Log("cd: " + d);
            Debug.Log(roofToward.Count);
            foreach (KeyValuePair<Direction, GameObject> kvp in roofToward) Debug.Log(kvp.Key + ":" + kvp.Value);
            Debug.Log("wtf");
            doors[d] = false;
            GameObject roof = roofToward[d];            
            roof.GetComponent<Renderer>().material = White;
            GameObject door = doorToward[d];
            door.SetActive(true);
            Debug.Log("cd: " + d.Name() + " door: " + door + ", roof: " + roof);
        }
        public void OpenAllDoors()
        {
            foreach (Direction d in Directions.NESW) OpenDoor(d);
        }
        public void CloseAllDoors()
        {
            Debug.Log(roofToward.Count);
            foreach (KeyValuePair<Direction, GameObject> kvp in roofToward) Debug.Log(kvp.Key + ":" + kvp.Value);
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
        #region hallways
        private bool _isHallway = false;
        public bool Hallway
        {
            get => _isHallway;
            set
            {
                if (value)
                {
                    if (!CanBeHallway)
                    {
                        Debug.LogError("Tried to make a hallway out of an invalid room!");
                    }
                    _isHallway = value;
                }
                else
                {
                    _isHallway = value;
                }
            }
        }
        public bool CanBeHallway
        {
            get
            {
                bool NS = doors[Direction.NORTH] || doors[Direction.SOUTH];
                bool EW = doors[Direction.EAST]  || doors[Direction.WEST];
                return (NS && !EW) || (EW && !NS);
            }
        }
        #endregion hallways
        #region generation
        void Awake()
        {
            foreach(Direction d in Directions.NESW)
            {
                GameObject door = transform.Find(d.Name() + " Door").gameObject;                
                GameObject roof = transform.Find(d.Name() + " Cardinal Roof").gameObject;
                Debug.Log(d.Name() + " door: " + door + ", roof: " + roof);
                doorToward[d] = door;
                roofToward[d] = roof;
            }
            Debug.Log(roofToward.Count);
        }
        public void PostStart((int x, int y) pos, bool initial = false)
        {
            Debug.Log(roofToward.Count);
            if (initial) OpenAllDoors();
            else
            {
                position = pos;
                MazeMaker.MazeRooms[pos] = this;
                CloseAllDoors();
                GenerateDoors();
            }
        }
        public void GenerateDoors()
        {
            int numDoors = UnityEngine.Random.Range(1, 4);
            for (int i = 0; i < numDoors; i++)
            {
                // note that this will produce duplicates sometimes. this is fine by me, since i want to bias toward fewer doors anyway.
                doors[UnityEngine.Random.Range(0, 3)] = true;
            }
            // add required doors
            MazeRoom temp;
            foreach (Direction d in Directions.NESW)
            {
                if ((temp = MazeMaker.RoomFrom(position, d)) != null && temp.HasDoorFacing(d)) OpenDoor(d);
            }
        }
        #endregion generation
    }
}
