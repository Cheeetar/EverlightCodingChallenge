using System;
using BallDropper.Enums;

namespace BallDropper
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Get arguments
            if (args.Length != 2)
            {
                throw new ArgumentException("Incorrect syntax. Usage: ./BallDropper [depth] [logLevel]");
            }

            if (!int.TryParse(args[0], out var depth) || depth < 1 || depth > 31)
            {
                throw new ArgumentException("Incorrect syntax. Depth must be an integer between 1 and 31 inclusive.");
            }

            if (!int.TryParse(args[1], out var logLevel) || logLevel < 0 || logLevel > 3)
            {
                throw new ArgumentException("Incorrect syntax. Log level must be an integer between 0 and 3, 0 being verbose debug, 1 being debug, 2 being info, and 3 being none.");
            }

            Logger.LogLevel = (LogLevel)logLevel;

            var (prediction, result) = DropBalls(depth);

            Logger.Log($"Prediction: {prediction}, Result: {result}", true);
        }

        public static (int, int) DropBalls(int depth)
        {

            // Setup model
            var tree = new Node(depth);
            var containerCount = Math.Pow(2, depth);

            // Get prediction
            var predictedEmptyContainer = PredictEmptyContainer(tree, depth);
            Logger.LogInfo($"Predicting the unpopulated node will be: {predictedEmptyContainer} of {containerCount}", true);

            // Run simulation
            SimulateDropping(tree, containerCount);

            // Print result of simulation
            var emptyContainer = GetEmptyContainer(tree);

            return (predictedEmptyContainer, emptyContainer);
        }

        public static void SimulateDropping(Node tree, double containerCount)
        {
            var balls = containerCount - 1;
            for (int ballsDropped = 0; ballsDropped < balls; ballsDropped += 1)
            {
                Logger.LogVerboseDebug("Dropping ball: ");
                tree.DropBall();
                Logger.LogVerboseDebug("", true);
            }
        }

        public static int PredictEmptyContainer(Node tree, int currentDepth)
        {
            var currentNode = tree;
            var prediction = 1;

            for (; currentDepth > 0; currentDepth -= 1)
            {
                // Go to whichever node is closed.
                switch (currentNode.OpenNode)
                {
                    case Direction.Left:
                        // Going right? Container will skip each of the containers to the left, the count of which can be calculated easily by checking how high up the tree we are.
                        prediction += (int)Math.Pow(2, currentDepth) / 2;
                        currentNode = currentNode.Right;
                        break;
                    case Direction.Right:
                        currentNode = currentNode.Left;
                        break;
                    default:
                        throw new Exception($"Unexpected direction for open node while predicting result: {currentNode.OpenNode}");
                }
            }

            return prediction;
        }

        public static int GetEmptyContainer(Node tree)
        {
            var containerNumber = 0;
            var emptyContainer = 0;
            var containers = tree.GetContainers();

            foreach(var container in containers)
            {
                containerNumber += 1;
                if (container.HasBall)
                {
                    Logger.LogDebug($"{containerNumber}: Ball", true);
                }
                else
                {
                    Logger.LogInfo($"{containerNumber}: No Ball", true);
                    emptyContainer = containerNumber;
                }
            }

            return emptyContainer;
        }
    }
}