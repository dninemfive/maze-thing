﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class MazeRoom : MonoBehaviour
    {
        public DirectionHolder doors = new DirectionHolder();
        public (int x, int y) position;

        public void OpenDoor(Direction door)
        {
            doors[door] = true;
        }
        public void CloseDoor(Direction door)
        {
            doors[door] = false;
        }
        public IEnumerable<Direction> DoorDirections
        {
            get
            {
                for (int i = 0; i < doors.Length; i++) if (doors[i]) yield return (Direction)i;
            }
        }
        public bool CanBeHallway
        {
            get
            {
                // because doors is in NESW order, indices of the same parity are in the same direction
                if (doors[Direction.NORTH] && doors[Direction.SOUTH] && !(doors[Direction.EAST] || doors[Direction.WEST])) return true;
                if (doors[Direction.EAST] && doors[Direction.WEST] && !(doors[Direction.SOUTH] || doors[Direction.NORTH])) return true;
                return false;
            }
        }
        private bool _isHallway = false;
        public bool HasDoorFacing(Direction d)
        {
            return doors[(int)d];
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
            for(int i = 0; i < 4; i++)
            {
                if ((temp = MazeMaker.RoomFrom(position, (Direction)i)) != null && temp.HasDoorFacing((Direction)i)) OpenDoor((Direction)i);
            }
        }
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
        void Start()
        {
            GenerateDoors();
        }
    }
    public class DirectionHolder
    {
        public int Length => directionsNESW.Length;
        private bool[] directionsNESW = { false, false, false, false };
        public bool this[int index]
        {
            get
            {
                return directionsNESW[index];
            }
            set
            {
                directionsNESW[index] = value;
            }            
        }
        public bool this[Direction d]
        {
            get
            {
                return this[(int)d];
            }
            set
            {
                this[(int)d] = value;
            }
        }
    }
}
