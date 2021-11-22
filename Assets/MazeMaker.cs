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
        const int MAX_RECURSION_DEPTH = 2;
        /// <summary>
        /// The field holding each room of the maze, indexed by their location.
        /// </summary>
        public static Dictionary<(int x, int y), MazeRoom> MazeRooms = new Dictionary<(int x, int y), MazeRoom>();
        /// <summary>
        /// The prefab to place when generating a new maze room. Set in Unity. Requires the <c>MazeRoom</c> component and subobjects with the following names:
        /// - [North|East|South|West] Cardinal Roof
        /// - [North|East|South|West] Door
        /// - Camera
        /// - Point Light
        /// </summary>
        public GameObject mazeRoomPrefab;
        /// <summary>
        /// The unique instance of the MazeMaker, which must not be static because it's a <c>MonoBehaviour</c>.        
        /// </summary>
        private static MazeMaker _singleton = null;
        public static MazeMaker Singleton
        {
            get => _singleton;
            private set { _singleton = value;  }
        }
        /// <summary>
        /// The materials used for representing open and non-open door directions, respectively. Non-static so they can be set directly in Unity.
        /// </summary>
        public Material white, black;
        /// <summary>
        /// Static references to the above materials, for convenience.
        /// </summary>
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
            if (iterations >= MAX_RECURSION_DEPTH || pos == (0, 0)) return;
            if(!MazeRooms.ContainsKey(pos) || MazeRooms[pos] == null)
            {
                GameObject newRoom = Instantiate(mazeRoomPrefab, new Vector3(10 * pos.x, 0, 10 * pos.y), Quaternion.identity);
                MazeRoom newMR = newRoom.AddComponent<MazeRoom>();
                newMR.PostStart(pos);
            }
            GenerateNeighbors(MazeRooms[pos], ++iterations);
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