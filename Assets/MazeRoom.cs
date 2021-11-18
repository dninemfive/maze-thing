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
        #region doors
        public void OpenDoor(Direction door)
        {
            doors[door] = true;
        }
        public void CloseDoor(Direction door)
        {
            doors[door] = false;
        }
        public void OpenAllDoors()
        {
            foreach (Direction d in Directions.NESW) OpenDoor(d);
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
            GenerateDoors();
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
