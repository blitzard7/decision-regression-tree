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
            var rootNode = new Node();
            rootNode.Feature = "Swimming Suit";
            var noneSwimmingSuitChild = new Node();
            noneSwimmingSuitChild.Children = new Dictionary<string, INode>();
            noneSwimmingSuitChild.CurrentClassification.Add("No", 2);

            var smallSwimmingSuidChild = new Node();
            smallSwimmingSuidChild.Children = new Dictionary<string, INode>();
            smallSwimmingSuidChild.CurrentClassification.Add("No", 2);
            rootNode.Children.Add("None", noneSwimmingSuitChild);
            rootNode.Children.Add("Small", smallSwimmingSuidChild);

            var temperatureNode = new Node();
            temperatureNode.Feature = "Water Temperature";
            temperatureNode.Children = new Dictionary<string, INode>();
            temperatureNode.CurrentClassification.Add("No", 1);
            temperatureNode.CurrentClassification.Add("Yes", 1);
            var coldSubNode = new Node();
            coldSubNode.CurrentClassification.Add("No", 1);

            var warmSubNode = new Node();
            warmSubNode.CurrentClassification.Add("Yes", 1);
            temperatureNode.Children.Add("Cold", coldSubNode);
            temperatureNode.Children.Add("Warm", warmSubNode);

            var goodSwimmingSuidChild = new Node();
            goodSwimmingSuidChild.Children.Add("Good", temperatureNode);
            rootNode.Children.Add("Good", temperatureNode);
            rootNode.CurrentClassification.Add("No", 5);
            rootNode.CurrentClassification.Add("Yes", 1);

            var dt = new Trees.DecisionTree();
            dt.Root = rootNode;

            // Act 
            var list = new List<(string, string)>();
            list.Add(("Swimming Suit", "Good"));
            list.Add(("Water Temperature", "Warm"));
            var node = dt.Query(list);

            // Assert
            Assert.Contains("None", node.Children.Keys);
        }
    }
}
