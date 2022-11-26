using Newtonsoft.Json;
using NUnit.Framework;

namespace Physalia.AbilityFramework.Tests
{
    public class GraphConverterTests
    {
        [NodeCategory("Built-in/[Test Custom]")]
        public class DamageNode : ProcessNode
        {
            public Inport<StatOwner> owners;
            public Inport<int> baseValue;
        }

        [NodeCategory("Built-in/[Test Custom]")]
        public class OwnerFilterNode : Node
        {
            public Outport<StatOwner> owners;
        }

        [NodeCategory("Built-in/[Test Custom]")]
        public class IntNode : Node
        {
            public Outport<StatOwner> output;
            public Variable<int> value;
        }

        [NodeCategory("Built-in/[Test Custom]")]
        public class LogNode : ProcessNode
        {
            public Inport<string> text;
        }

        [Test]
        public void Serialize_Normal()
        {
            Graph graph = new Graph();

            StartNode startNode = graph.AddNewNode<StartNode>();
            DamageNode damageNode = graph.AddNewNode<DamageNode>();
            OwnerFilterNode filterNode = graph.AddNewNode<OwnerFilterNode>();
            IntNode intNode = graph.AddNewNode<IntNode>();
            LogNode logNode = graph.AddNewNode<LogNode>();

            startNode.next.Connect(damageNode.previous);
            damageNode.next.Connect(logNode.previous);
            damageNode.owners.Connect(filterNode.owners);
            damageNode.baseValue.Connect(intNode.output);

            // Intentionally change node id for easier test
            startNode.id = 1;
            damageNode.id = 2;
            filterNode.id = 3;
            intNode.id = 4;
            logNode.id = 5;

            var expected =
                "{\"_type\":\"Physalia.AbilityFramework.Graph\"," +
                "\"variables\":[]," +
                "\"nodes\":[{\"_id\":1,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.StartNode\"}," +
                "{\"_id\":2,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+DamageNode\"}," +
                "{\"_id\":3,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+OwnerFilterNode\"}," +
                "{\"_id\":4,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+IntNode\",\"value\":0}," +
                "{\"_id\":5,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+LogNode\"}]," +
                "\"edges\":[{\"id1\":1,\"port1\":\"next\",\"id2\":2,\"port2\":\"previous\"}," +
                "{\"id1\":2,\"port1\":\"next\",\"id2\":5,\"port2\":\"previous\"}," +
                "{\"id1\":3,\"port1\":\"owners\",\"id2\":2,\"port2\":\"owners\"}," +
                "{\"id1\":4,\"port1\":\"output\",\"id2\":2,\"port2\":\"baseValue\"}]}";

            string json = JsonConvert.SerializeObject(graph);
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void Deserialize_Normal()
        {
            var json =
                "{\"_type\":\"Physalia.AbilityFramework.Graph\"," +
                "\"nodes\":[{\"_id\":1,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.StartNode\"}," +
                "{\"_id\":2,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+DamageNode\"}," +
                "{\"_id\":3,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+OwnerFilterNode\"}," +
                "{\"_id\":4,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+IntNode\",\"value\":0}," +
                "{\"_id\":5,\"_position\":{\"x\":0.0,\"y\":0.0},\"_type\":\"Physalia.AbilityFramework.Tests.GraphConverterTests+LogNode\"}]," +
                "\"edges\":[{\"id1\":1,\"port1\":\"next\",\"id2\":2,\"port2\":\"previous\"}," +
                "{\"id1\":2,\"port1\":\"next\",\"id2\":5,\"port2\":\"previous\"}," +
                "{\"id1\":2,\"port1\":\"owners\",\"id2\":3,\"port2\":\"owners\"}," +
                "{\"id1\":2,\"port1\":\"baseValue\",\"id2\":4,\"port2\":\"output\"}]}";

            Graph graph = JsonConvert.DeserializeObject<Graph>(json);

            Assert.AreEqual(5, graph.Nodes.Count);
            Assert.AreEqual(true, graph.Nodes[0] is StartNode);
            Assert.AreEqual(true, graph.Nodes[1] is DamageNode);
            Assert.AreEqual(true, graph.Nodes[2] is OwnerFilterNode);
            Assert.AreEqual(true, graph.Nodes[3] is IntNode);
            Assert.AreEqual(true, graph.Nodes[4] is LogNode);

            var startNode = graph.Nodes[0] as StartNode;
            var damageNode = graph.Nodes[1] as DamageNode;
            var filterNode = graph.Nodes[2] as OwnerFilterNode;
            var intNode = graph.Nodes[3] as IntNode;
            var logNode = graph.Nodes[4] as LogNode;

            Assert.AreEqual(1, graph.EntryNodes.Count);
            Assert.AreEqual(startNode, graph.EntryNodes[0]);

            Assert.AreEqual(damageNode, startNode.Next);
            Assert.AreEqual(startNode, damageNode.Previous);

            Assert.AreEqual(logNode, damageNode.Next);
            Assert.AreEqual(damageNode, logNode.Previous);

            Assert.AreEqual(filterNode.owners, damageNode.owners.GetConnections()[0]);
            Assert.AreEqual(intNode.output, damageNode.baseValue.GetConnections()[0]);
        }

        [Test]
        public void Deserialize_WithMissingPort()
        {
            var json =
                "{\"_type\":\"Physalia.AbilityFramework.AbilityGraph\"," +
                "\"nodes\":[{\"_id\":1,\"_type\":\"Physalia.AbilityFramework.StartNode\"}," +
                "{\"_id\":2,\"_type\":\"Physalia.AbilityFramework.LogNode\"}," +
                "{\"_id\":3,\"_type\":\"Physalia.AbilityFramework.StringNode\"}]," +
                "\"edges\":[{\"id1\":1,\"port1\":\"next\",\"id2\":2,\"port2\":\"previous\"}," +
                "{\"id1\":3,\"port1\":\"value\",\"id2\":2,\"port2\":\"text\"}]}";

            Graph graph = JsonConvert.DeserializeObject<Graph>(json);

            Assert.AreEqual(3, graph.Nodes.Count);
            Assert.AreEqual(true, graph.Nodes[0] is StartNode);
            Assert.AreEqual(true, graph.Nodes[1] is AbilityFramework.LogNode);
            Assert.AreEqual(true, graph.Nodes[2] is StringNode);

            var startNode = graph.Nodes[0] as StartNode;
            var logNode = graph.Nodes[1] as AbilityFramework.LogNode;
            var stringNode = graph.Nodes[2] as StringNode;

            Assert.AreEqual(1, graph.EntryNodes.Count);
            Assert.AreEqual(startNode, graph.EntryNodes[0]);

            Assert.AreEqual(logNode, startNode.Next);
            Assert.AreEqual(startNode, logNode.Previous);

            var missingPort = stringNode.GetOutput("value") as MissingOutport;
            Assert.NotNull(missingPort);
            Assert.AreEqual(logNode.text, missingPort.GetConnections()[0]);
        }
    }
}
