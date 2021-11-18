using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3 {
    public enum Direction : byte
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
    }
    public class MazeMaker : MonoBehaviour
    {
        const int MAX_RECURSION_DEPTH = 4;
        public static Dictionary<(int x, int y), MazeRoom> MazeRooms = new Dictionary<(int x, int y), MazeRoom>();
        public GameObject mazeRoomPrefab;
        // not the same as DoorDirections mostly because i forgot, but also because they don't need to be interchangeable and it's more readable this way
        
        public void GenerateNeighbors(MazeRoom room, int iterations = 0)
        {
            foreach (Direction d in room.DoorDirections)
            {
                GenerateRoom(CoordsFrom(room.position, d), iterations);
            }
        }
        public void GenerateRoom((int x, int y) pos, int iterations = 0)
        {
            if (iterations >= MAX_RECURSION_DEPTH) return;
            GameObject newRoom = Instantiate(mazeRoomPrefab, new Vector3(10 * pos.x, 0, 10 * pos.y), Quaternion.identity);
            MazeRoom newMR = newRoom.AddComponent<MazeRoom>();
            newMR.position = pos;
            MazeRooms[pos] = newMR;
            GenerateNeighbors(newMR, ++iterations);
        }

        public static MazeRoom[] NeighborsOf((int x, int y) pos) 
        {
            return new MazeRoom[] { RoomFrom(pos, Direction.NORTH),
                                    RoomFrom(pos, Direction.EAST),
                                    RoomFrom(pos, Direction.SOUTH),
                                    RoomFrom(pos, Direction.WEST) };
        }

        public static MazeRoom RoomAt((int x, int y) pos)
        {
            if (MazeRooms.ContainsKey(pos)) return MazeRooms[pos];
            return null;
        }
        public static MazeRoom RoomFrom((int x, int y) pos, Direction d)
        {
            return RoomAt(CoordsFrom(pos, d));
        }
        public static (int x, int y) CoordsFrom((int x, int y) pos, Direction d)
        {
            var (x, y) = pos;
            switch (d)
            {
                case Direction.NORTH:
                    return (x, y + 1);
                case Direction.SOUTH:
                    return (x, y - 1);
                case Direction.EAST:
                    return (x + 1, y);
                case Direction.WEST:
                    return (x - 1, y);
                default: throw new ArgumentNullException();
            }
        }
        private void Start()
        {
            MazeRoom center = GameObject.Find("MazeRoom").GetComponent<MazeRoom>();
            (int x, int y) pos = (0, 0);
            MazeRooms[pos] = center;
            center.OpenAllDoors();
            GenerateNeighbors(center);
        }
    }    
}