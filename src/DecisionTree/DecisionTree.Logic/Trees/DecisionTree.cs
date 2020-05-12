using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Trees
{
    /* TODO:
     * Query Tree and return path
     * Node should be adapted to contain the resultValue
     */

    public class DecisionTree : IDecisionTree
    {
        public INode Root { get; set; }

        // Build tree from given CsvData
        // Recursive Task?
        // Entropy and InformationGain is calculated 
        // Stop groving after all leafe nodes are homogeneouse (Post-pruning)
        // Returns DecisionTree
        public ITree BuildTree(CsvData data)
        {
            // 1. determine root node (column) of tree
            // 2. calculate for each search key of current node (first node is root) the entropy and IG for next node (child)
            // 3. repeat 2 until no split is needed (meaning that all leafe nodes contain homogeneous data)

            // Assumption: Last column represents query result.
            // For E(G) we have to extract all result values 
            Root = new Node();
            Root.Start(data);
            
            return this;
        }

        public void PostPruning()
        {
            throw new NotImplementedException();
        }

        public INode Query(List<(string featureName, string featureValue)> searchKeys)
        {
            // If there is no tree then return null.
            if (Root == null)
            {
                return null;
            }

            var tmpRoot = Root;
            var queryNode = new Node();
            foreach (var (featureName, featureValue) in searchKeys)
            {
                var tmpNode = new Node();
                tmpNode.Children = GetSubsetOfNode(tmpRoot, (featureName, featureValue));
                var a = FindPaths(searchKeys, (featureName, featureValue), tmpNode.Children);
                if (tmpRoot.Feature == featureName)
                {
                    tmpNode.Feature = featureName;
                    var subNode = tmpRoot.Children[featureValue];
                    var tmpSub = new Node();
                    tmpSub.Children = GetSubsetOfNode(subNode, (featureName, featureValue));
                    tmpRoot = subNode;
                    tmpNode.Children.Add(featureValue, subNode);
                }

                queryNode.Children.Add(tmpNode.Feature, tmpNode);
            }

            // Search keys should be like:
            // (string featureName, string value) e.g. (Outlook, sunny) 
            // therefore we expect as input a List<(string,string)>
            return queryNode;
        }

        private List<List<string>> FindPaths(List<(string featureName, string featureVal)> seachkeys, (string featureName, string featureVal) currentKey , Dictionary<string, INode> tree)
        {
            List<List<string>> paths = new List<List<string>>();

            foreach (var (featureName, featureVal) in seachkeys)
            {
                if (tree.ContainsKey(currentKey.featureVal))
                {
                    var treeNode = tree[currentKey.featureVal];
                    foreach (var node in treeNode.Children)
                    {
                        var subPaths = FindPaths(seachkeys, (featureName, featureVal), tree);
                        foreach (var subPath in subPaths)
                        {
                            subPath.Insert(0, currentKey.featureVal);
                            paths.Add(subPath);
                        }
                    }
                }
                else
                {
                    paths.Add(new List<string>() { currentKey.featureVal });
                }
            }
           
            return paths;
        }

        private void findPaths(int root, Dictionary<int, List<int>> tree, List<List<int>> pathList, List<int> visitedNodes)
        {
            visitedNodes.Add(root);
            if (tree.ContainsKey(root))
            {
                foreach (int v in tree[root])
                {
                    findPaths(v, tree, pathList, visitedNodes);
                    visitedNodes.RemoveAt(visitedNodes.Count - 1);
                }
            }
            else
            {
                pathList.Add(new List<int>(visitedNodes));
            }
        }

        private Dictionary<string, INode> GetSubsetOfNode(INode node, (string featureName, string featureValue) searchKey)
        {
            var subset = node.Children.Where(s => s.Key == searchKey.featureValue)
                        .ToDictionary(dict => dict.Key, dict => dict.Value);

            return subset;
        }
    }
}
