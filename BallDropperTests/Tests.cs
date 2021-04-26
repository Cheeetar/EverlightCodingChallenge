using BallDropper;
using BallDropper.Enums;
using NUnit.Framework;
using System.Linq;

namespace BallDropperTests
{
    // Tests are run from a depth of 1 to 18, to avoid taking too long.
    // Theoretically should work up to a depth of 31 (32 will overflow the count of containers).
    // Not good: The tests simulate pseudorandom models each time. Ideally, we'd have a mock tree which would always have the same
    // open and closed gates, and we'd then always know the correct answer.
    [TestFixture]
    internal class Tests
    {
        [OneTimeSetUp]
        public void InitialSetup()
        {
            Logger.LogLevel = LogLevel.VerboseDebug;
        }

        [Test]
        public void EachContainerButOneGetsOneBall([Range(1, 18)] int treeDepth)
        {
            var tree = new Node(treeDepth);
            Program.SimulateDropping(tree, GetContainerCount(tree));
            var containers = tree.GetContainers();
            var filledContainers = containers.Where(container => container.HasBall);
            var emptyContainers = containers.Where(container => !container.HasBall);

            Assert.AreEqual(GetContainerCount(tree) - 1, filledContainers.Count());
            Assert.AreEqual(1, emptyContainers.Count());
        }

        [Test]
        public void SimulationCorrect([Range(1, 18)] int treeDepth)
        {
            var tree = new Node(treeDepth);
            Program.SimulateDropping(tree, GetContainerCount(tree));
            var emptyNode = Program.GetEmptyContainer(tree);
            var emptyContainer = GetContainer(tree, emptyNode - 1);

            Assert.IsFalse(emptyContainer.HasBall);
        }

        [Test]
        public void PredictionsMatchSimulation([Range(1, 18)] int treeDepth)
        {
            var tree = new Node(treeDepth);
            var prediction = Program.PredictEmptyContainer(tree, treeDepth);
            Program.SimulateDropping(tree, GetContainerCount(tree));
            var emptyNode = Program.GetEmptyContainer(tree);
 
            Assert.AreEqual(prediction, emptyNode);
        }

        public static int GetContainerCount(Node tree) => tree.GetContainers().Count();

        public static Node GetContainer(Node tree, int childContainerIndex) => tree.GetContainers().ToList()[childContainerIndex];
    }
}
