using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dninemfive.cmpm121.p3
{
    public enum Direction : byte
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
    }
    public class Directions
    {
        public int Length => heldDirections.Length;
        private readonly bool[] heldDirections = { false, false, false, false };
        public bool this[int index]
        {
            get
            {
                return heldDirections[index];
            }
            set
            {
                heldDirections[index] = value;
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

        public static IEnumerable<Direction> NESW
        {
            get
            {
                yield return Direction.NORTH;
                yield return Direction.EAST;
                yield return Direction.SOUTH;
                yield return Direction.WEST;
            }
        }        
    }
    public static class DirectionUtility
    {
        public static string Name(this Direction d)
        {
            switch (d)
            {
                case Direction.NORTH: return "North";
                case Direction.EAST: return "East";
                case Direction.SOUTH: return "South";
                case Direction.WEST: return "West";
                default: throw new ArgumentOutOfRangeException();
            }
        }
        public static int Int(this Direction d) => (int)d;
    }
}
