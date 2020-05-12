using DecisionTree.Logic.Trees;
using System.Collections.Generic;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class DecisionTreeTest
    {
        [Fact]
        public void Query_ShouldReturnNodeWithPathToClassificationOfQueriedInput()
        {
            // Arrange
            var rootNode = new Node
            {
                Parent = null,
                Feature = "Swimming Suit"
            };
            var noneSwimmingSuitChild = new Node
            {
                Parent = rootNode,
                Children = new Dictionary<string, INode>()
            };
            noneSwimmingSuitChild.CurrentClassification.Add("No", 2);

            var smallSwimmingSuidChild = new Node
            {
                Parent = rootNode,
                Children = new Dictionary<string, INode>()
            };
            smallSwimmingSuidChild.CurrentClassification.Add("No", 2);
            rootNode.Children.Add("None", noneSwimmingSuitChild);
            rootNode.Children.Add("Small", smallSwimmingSuidChild);

            var temperatureNode = new Node
            {
                Parent = rootNode,
                Feature = "Water Temperature",
                Children = new Dictionary<string, INode>()
            };
            temperatureNode.CurrentClassification.Add("No", 1);
            temperatureNode.CurrentClassification.Add("Yes", 1);
            var coldSubNode = new Node
            {
                Parent = temperatureNode
            };
            coldSubNode.CurrentClassification.Add("No", 1);

            var warmSubNode = new Node
            {
                Parent = temperatureNode
            };
            warmSubNode.CurrentClassification.Add("Yes", 1);
            temperatureNode.Children.Add("Cold", coldSubNode);
            temperatureNode.Children.Add("Warm", warmSubNode);

            var goodSwimmingSuidChild = new Node
            {
                Parent = rootNode
            };
            goodSwimmingSuidChild.Children.Add("Good", temperatureNode);
            rootNode.Children.Add("Good", temperatureNode);
            rootNode.CurrentClassification.Add("No", 5);
            rootNode.CurrentClassification.Add("Yes", 1);

            var dt = new Trees.DecisionTree
            {
                Root = rootNode
            };

            // Act 
            var list = new List<(string, string)>
            {
                ("Swimming Suit", "None"),
                ("Water Temperature", "Warm")
            };

            var node = dt.Query(list);

            // Assert
            Assert.Contains("None", node.Children.Keys);
        }
    }
}
