using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

// array, list                  https://docs.microsoft.com/en-us/previous-versions/ms379570%28v%3dvs.80%29
// queue, stack, hashset        https://docs.microsoft.com/en-us/previous-versions/ms379571%28v%3dvs.80%29
// binary trees                 https://docs.microsoft.com/en-us/previous-versions/ms379572%28v%3dvs.80%29
// avl/redblack trees, skiplist https://docs.microsoft.com/en-us/previous-versions/ms379573%28v%3dvs.80%29
// graphs                       https://docs.microsoft.com/en-us/previous-versions/ms379574%28v%3dvs.80%29
// sets                         https://docs.microsoft.com/en-us/previous-versions/ms379575%28v%3dvs.80%29
namespace Graph
{
    public class GraphTests
    {

        [Fact]
        public void ConstructGraphTest()
        {
            var graph = this.GetGraphData();
            Assert.NotNull(graph);
        }
        
        //https://en.wikipedia.org/wiki/Depth-first_search
        private string[] DfsExpected = {"Frankfurt", "Manheim", "Karlsruhe", "Munchen", "Wurzburg", "Erfurt", "Nurnberg", "Stuttgart", "Kassel"};
        [Fact]
        public void GraphDfsTest()
        {
        }

        // https://en.wikipedia.org/wiki/Breadth-first_search
        private string[] BfsExpected = {"Frankfurt", "Manheim", "Karlsruhe", "Augsburg", "Wurzburg", "Nurnberg", "Suttgart", "Erfurt","Kassel","Munchen"};
        [Fact]
        public void GraphBfsTest()
        {
            var citiesGraph = this.GetSearchGraphData();
            Assert.Equal(10, citiesGraph.Count);
            Assert.Equal(11*2, citiesGraph.GraphNodes.Sum(i => i.Neighbors.Count));
        }

        private Graph<string> GetSearchGraphData()
        {
            Graph<string> cities = new Graph<string>();
            cities.AddNode("Frankfurt");
            cities.AddNode("Mannheim");
            cities.AddNode("Wurzburg");
            cities.AddNode("Stuttgart");
            cities.AddNode("Kassel");
            cities.AddNode("Karlsruhe");
            cities.AddNode("Erfurt");
            cities.AddNode("Numberg");
            cities.AddNode("Augsburg");
            cities.AddNode("Munchen");

            cities.AddUndirectedEdge("Frankfurt", "Mannheim", 85);
            cities.AddUndirectedEdge("Mannheim", "Karlsruhe", 80);
            cities.AddUndirectedEdge("Karlsruhe", "Augsburg", 250);
            cities.AddUndirectedEdge("Augsburg", "Munchen", 84);

            cities.AddUndirectedEdge("Frankfurt", "Wurzburg", 217);
            cities.AddUndirectedEdge("Wurzburg", "Erfurt", 186);
            cities.AddUndirectedEdge("Wurzburg", "Numberg", 103);
            cities.AddUndirectedEdge("Numberg", "Munchen", 167);

            cities.AddUndirectedEdge("Frankfurt", "Kassel", 173);
            cities.AddUndirectedEdge("Kassel", "Munchen", 502);

            cities.AddUndirectedEdge("Stuttgart", "Numberg", 183);

            return cities;
        }

        private Graph<string> GetGraphData()
        {
            Graph<string> web = new Graph<string>();
            web.AddNode("Privacy.htm");
            web.AddNode("People.aspx");
            web.AddNode("About.htm");
            web.AddNode("Index.htm");
            web.AddNode("Products.aspx");
            web.AddNode("Contact.aspx");

            web.AddDirectedEdge("People.aspx", "Privacy.htm");  // People -> Privacy

            web.AddDirectedEdge("Privacy.htm", "Index.htm");    // Privacy -> Index
            web.AddDirectedEdge("Privacy.htm", "About.htm");    // Privacy -> About

            web.AddDirectedEdge("About.htm", "Privacy.htm");    // About -> Privacy
            web.AddDirectedEdge("About.htm", "People.aspx");    // About -> People
            web.AddDirectedEdge("About.htm", "Contact.aspx");   // About -> Contact

            web.AddDirectedEdge("Index.htm", "About.htm");      // Index -> About
            web.AddDirectedEdge("Index.htm", "Contact.aspx");   // Index -> Contacts
            web.AddDirectedEdge("Index.htm", "Products.aspx");  // Index -> Products

            web.AddDirectedEdge("Products.aspx", "Index.htm");  // Products -> Index
            web.AddDirectedEdge("Products.aspx", "People.aspx");// Products -> People

            return web;
        }
    }
}
