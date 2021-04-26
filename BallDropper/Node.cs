using BallDropper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BallDropper
{
    public class Node
    {
        public Node(int depth)
        {
            Depth = depth;
            if (depth > 0)
            {
                var newDepth = depth - 1;
                Left = new Node(newDepth);
                Right = new Node(newDepth);
                OpenNode = (Direction)rng.Next(1, 3); // Note: Not cryptographically secure. Doesn't particularly matter for this model.
            }
            else
            {
                HasBall = false;
                OpenNode = Direction.None;
            }
        }

        private static Random rng = new Random();

        private int Depth { get; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public bool HasBall { get; set; }

        public Direction OpenNode { get; set; }

        public void DropBall()
        {
            switch(OpenNode)
            {
                case Direction.Left:
                    Logger.LogVerboseDebug("Left > ");
                    Left.DropBall();
                    OpenNode = Direction.Right;
                    break;
                case Direction.Right:
                    Logger.LogVerboseDebug("Right > ");
                    Right.DropBall();
                    OpenNode = Direction.Left;
                    break;
                case Direction.None:
                    Logger.LogVerboseDebug("Bucket");
                    HasBall = true;
                    break;
                default:
                    throw new Exception($"Unexpected direction for open node: {OpenNode}");
            }
        }

        public IEnumerable<Node> GetContainers()
        {
            var currentDepth = Depth;
            IEnumerable<Node> childNodes = new List<Node>() { Left, Right };
            for (; currentDepth > 1; currentDepth -= 1)
            {
                childNodes = childNodes.SelectMany(childNode => new List<Node>() { childNode.Left, childNode.Right });
            }
            return childNodes;
        }
    }
}