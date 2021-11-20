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
        public Dictionary<Direction, (GameObject door, GameObject roof)> objectsToward = new Dictionary<Direction, (GameObject door, GameObject roof)>();
        #region doors
        public void OpenDoor(Direction d)
        {
            doors[d] = true;
            var (door, roof) = objectsToward[d];
            roof.GetComponent<Renderer>().material = Black;
            door.SetActive(false);
        }
        public void CloseDoor(Direction d)
        {
            Debug.Log("cd: " + d.Name());
            doors[d] = false;
            var (door, roof) = objectsToward[d];
            Debug.Log("cd: " + d.Name() + " door: " + door + ", roof: " + roof);
            roof.GetComponent<Renderer>().material = White;
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
        void Start()
        {
            foreach(Direction d in Directions.NESW)
            {
                GameObject door = transform.Find(d.Name() + " Door").gameObject;                
                GameObject roof = transform.Find(d.Name() + " Cardinal Roof").gameObject;
                Debug.Log(d.Name() + " door: " + door + ", roof: " + roof);
                objectsToward[d] = (door, roof);
            }
            foreach (KeyValuePair<Direction, (GameObject door, GameObject roof)> kvp in objectsToward.AsEnumerable()) Debug.Log(kvp.Key + " " + kvp.Value);
        }
        public void PostStart(bool initial = false)
        {
            if (initial) OpenAllDoors();
            else
            {
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
