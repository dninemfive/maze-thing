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
    /// <summary>
    /// Wrapper class for the <c>Direction</c> enum, allowing more readable direct accesses of a set of cardinal directions and more readable direction enumeration.
    /// </summary>
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
    /// <summary>
    /// Contains extension methods for the <c>Direction</c> enum.
    /// </summary>
    public static class DirectionUtility
    {
        // This could probably be replaced with the implicit string conversion for enums, but I forgot that existed.
        // Would require changing the enum capitalization to Firstonly rather than ALLCAPS.
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
        public static Direction Opposite(this Direction d)
        {
            switch (d)
            {
                case Direction.NORTH: return Direction.SOUTH;
                case Direction.EAST: return Direction.WEST;
                case Direction.SOUTH: return Direction.NORTH;
                case Direction.WEST: return Direction.EAST;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
