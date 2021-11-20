using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3 {    
    public class MazeMaker : MonoBehaviour
    {
        /// <summary>
        /// Maximum recursion depth. Controls how many neighbors, neighbor-neighbors, &c are generated each time a new room is entered.
        /// </summary>
        const int MAX_RECURSION_DEPTH = 4;
        /// <summary>
        /// The field holding each room of the maze, indexed by their location.
        /// </summary>
        public static Dictionary<(int x, int y), MazeRoom> MazeRooms = new Dictionary<(int x, int y), MazeRoom>();
        /// <summary>
        /// The prefab to place when generating a new maze room. Set in Unity. Requires the MazeRoom component and subobjects with the following names:
        /// - [North|East|South|West] Cardinal Roof
        /// - [North|East|South|West] Door
        /// </summary>
        public GameObject mazeRoomPrefab;
        /// <summary>
        /// The instance of the current MazeMaker. Should never be necessary, mainly included to prevent creation of multiple MazeMaker instances.
        /// </summary>
        private static MazeMaker _singleton = null;
        public static MazeMaker Singleton
        {
            get => _singleton;
            private set { _singleton = value;  }
        }
        public Material white, black;
        public static Material White => Singleton.white;
        public static Material Black => Singleton.black;

        #region generation
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
            newMR.PostStart(pos);
            GenerateNeighbors(newMR, ++iterations);
        }
        #endregion generation
        #region inspection
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
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion inspection
        private void Start()
        {
            if (Singleton != null)
            {
                Destroy(this);
            } else
            {
                Singleton = this;
                MazeRoom center = GameObject.Find("MazeRoom").GetComponent<MazeRoom>();
                center.PostStart((0, 0), true);
                GenerateNeighbors(center);
            }            
        }
    }    
}