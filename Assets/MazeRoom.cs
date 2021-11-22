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
                doorToward[d] = door;
                roofToward[d] = roof;
            }
        }
        public void PostStart((int x, int y) pos, bool initial = false)
        {
            transform.Find("Center Roof/CoordDisplay").gameObject.GetComponent<TextMeshPro>().SetText("(" + pos.x + "," + pos.y + ")");
            position = pos;
            MazeMaker.MazeRooms[pos] = this;
            if (initial) OpenAllDoors();
            else
            {                
                CloseAllDoors();
                GenerateDoors();
            }
            Debug.Log("Room at " + position + " has in-game coords " + transform.position);
        }
        public void GenerateDoors()
        {
            int numDoors = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < numDoors; i++)
            {
                // note that this will produce duplicates sometimes. this is fine by me, since i want to bias toward fewer doors anyway.
                OpenDoor((Direction)UnityEngine.Random.Range(0, 3));
            }
            // add required doors
            MazeRoom temp;
            Debug.Log("Adding required doors for room at " + position + ": ");
            foreach (Direction d in Directions.NESW)
            {
                temp = MazeMaker.RoomFrom(position, d);
                if(temp != null)
                {
                    if(temp.HasDoorFacing(d.Opposite()))
                    {
                        Debug.Log("room at " + temp.position + ", " + d + " of room at " + position + " has door facing " + d.Opposite());
                        OpenDoor(d);
                    }
                    else
                    {
                        Debug.Log("room at " + temp.position + ", " + d + " of room at " + position + " does not have door facing " + d.Opposite());
                    }
                } else
                {
                    Debug.Log("could not find room at " + MazeMaker.CoordsFrom(position, d) + ", " + d + " of room at " + position + ".");
                }
            }
        }
        #endregion generation
    }
}
