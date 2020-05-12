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
            var noneSwimmingSuidChild = new Node();
            noneSwimmingSuidChild.Children = new Dictionary<string, INode>();
            noneSwimmingSuidChild.CurrentClassification.Add("No", 2);

            var smallSwimmingSuidChild = new Node();
            smallSwimmingSuidChild.Children = new Dictionary<string, INode>();
            smallSwimmingSuidChild.CurrentClassification.Add("No", 2);
            rootNode.Children.Add("None", noneSwimmingSuidChild);
            rootNode.Children.Add("Small", smallSwimmingSuidChild);

            var temperatureNode = new Node();
            temperatureNode.Feature = "Water Temperature";
            temperatureNode.Children = new Dictionary<string, INode>();
            temperatureNode.CurrentClassification.Add("No", 1);
            temperatureNode.CurrentClassification.Add("Yes", 1);
            var coldSubNode = new Node();
            coldSubNode.Children = null;
            coldSubNode.CurrentClassification.Add("No", 1);

            var warmSubNode = new Node();
            warmSubNode.Children = null;
            warmSubNode.CurrentClassification.Add("Yes", 1);
            temperatureNode.Children.Add("Cold", coldSubNode);
            temperatureNode.Children.Add("Warm", warmSubNode);

            var goodSwimmingSuidChild = new Node();
            goodSwimmingSuidChild.Children.Add("Good", temperatureNode);
            rootNode.Children.Add("Good", goodSwimmingSuidChild);

            // Act 

            // Assert
        }
    }
}
