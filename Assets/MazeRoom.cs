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
        [Flags]
        public enum DoorLocations : Byte
        {
            NONE = 0,
            NORTH = 1,
            EAST = 2,
            SOUTH = 4,
            WEST = 8
        }
        public DoorLocations doors;
        public (int x, int y) position;

        public void ToggleDoor(DoorLocations door)
        {
            // xoring a flag toggles it
            doors ^= door;
        }
        public IEnumerable<MazeMaker.Direction> DoorDirections
        {
            get
            {
                if ((doors & DoorLocations.NORTH) == DoorLocations.NORTH) yield return MazeMaker.Direction.NORTH;
                if ((doors & DoorLocations.EAST) == DoorLocations.EAST) yield return MazeMaker.Direction.EAST;
                if ((doors & DoorLocations.SOUTH) == DoorLocations.SOUTH) yield return MazeMaker.Direction.SOUTH;
                if ((doors & DoorLocations.WEST) == DoorLocations.WEST) yield return MazeMaker.Direction.WEST;
            }
        }
        public bool CanBeHallway => doors == (DoorLocations.NORTH | DoorLocations.SOUTH) || doors == (DoorLocations.EAST | DoorLocations.WEST);
        private bool _isHallway = false;
        public bool HasDoorFacing(MazeMaker.Direction d)
        {
            switch (d)
            {
                case MazeMaker.Direction.NORTH:
                    return (doors & DoorLocations.NORTH) == DoorLocations.NORTH;
                case MazeMaker.Direction.EAST:
                    return (doors & DoorLocations.EAST) == DoorLocations.EAST;
                case MazeMaker.Direction.SOUTH:
                    return (doors & DoorLocations.SOUTH) == DoorLocations.SOUTH;
                case MazeMaker.Direction.WEST:
                    return (doors & DoorLocations.WEST) == DoorLocations.WEST;
                default: return false;
            }
        }
        public void GenerateDoors()
        {
            int numDoors = UnityEngine.Random.Range(1, 4);
            for (int i = 0; i < numDoors; i++)
            {
                // just writing this because it's cursed tbh
                // note that this will produce duplicates sometimes. this is fine by me, since i want to bias toward fewer doors anyway.
                doors |= (DoorLocations)(int)Math.Pow(2, UnityEngine.Random.Range(0, 3));
            }
            // add required doors
            MazeRoom temp;
            if ((temp = MazeMaker.RoomFrom(position, MazeMaker.Direction.SOUTH)) != null && temp.HasDoorFacing(MazeMaker.Direction.NORTH)) doors |= DoorLocations.SOUTH;
            if ((temp = MazeMaker.RoomFrom(position, MazeMaker.Direction.WEST)) != null && temp.HasDoorFacing(MazeMaker.Direction.EAST)) doors |= DoorLocations.WEST;
            if ((temp = MazeMaker.RoomFrom(position, MazeMaker.Direction.NORTH)) != null && temp.HasDoorFacing(MazeMaker.Direction.SOUTH)) doors |= DoorLocations.NORTH;
            if ((temp = MazeMaker.RoomFrom(position, MazeMaker.Direction.EAST)) != null && temp.HasDoorFacing(MazeMaker.Direction.WEST)) doors |= DoorLocations.EAST;
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
        public MazeRoom((int x, int y) pos)
        {
            position = pos;
            GenerateDoors();
        }
    }
}
