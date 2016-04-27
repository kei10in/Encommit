using Encommit.Models.HistoryGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Encommit.Test.UnitTest
{
    public class TestGraphRenderer
    {
        [Fact]
        public void InitialRepository()
        {
            //
            // *
            //
            var commit = new HistoryCommit<string>(id: "1");
            var incomings = new List<string>();

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                Enumerable.Empty<HistoryEdge>(),
                Enumerable.Empty<HistoryEdge>(),
                Enumerable.Empty<string>());

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void NewestStandardCommit()
        {
            //
            // *
            // |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string>();

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                Enumerable.Empty<HistoryEdge>(),
                new[] { new HistoryEdge(0, 0) },
                new[] { "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void NewestMergeCommit()
        {
            //
            // *-\
            // | |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2", "3" });
            var incomings = new List<string>();

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                Enumerable.Empty<HistoryEdge>(),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(0, 1) },
                new[] { "2", "3" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void StandardCommitWithSingleChild()
        {
            // |
            // *
            // |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "1" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                new[] { new HistoryEdge(0, 0) },
                new[] { new HistoryEdge(0, 0) },
                new[] { "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void StandardCommitWithTwoChildren()
        {
            // |_/
            // *
            // |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "1", "1" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 0) },
                new[] { new HistoryEdge(0, 0) },
                new[] { "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void StandardCommitWithThreeChildren()
        {
            // |_/_/
            // *
            // |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "1", "1", "1" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 0), new HistoryEdge(2, 0) },
                new[] { new HistoryEdge(0, 0) },
                new[] { "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void StandardCommitWithParallelRightBranch()
        {
            // | |
            // * |
            // | |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "1", "2" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { "2", "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void StandardCommitWithParallelLeftBranch()
        {
            // | |
            // | *
            // | |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "2", "1" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(1),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { "2", "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void StandardCommitWithTwoChildrenAndParallelBranch()
        {
            // |_/ |
            // *  _/
            // | |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "1", "1", "2" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 0), new HistoryEdge(2, 1) },
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { "2", "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }

        [Fact]
        public void MergeCommitWithParallelBranch()
        {
            // | |
            // *-|-\
            // | | |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2", "3" });
            var incomings = new List<string> { "1", "4" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(0),
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1), new HistoryEdge(0, 2) },
                new[] { "2", "4", "3" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }


        [Fact]
        public void StandardCommitWithoutChildren()
        {
            // | 
            // | *
            // | |
            var commit = new HistoryCommit<string>(id: "1", parentes: new List<string> { "2" });
            var incomings = new List<string> { "3" };

            var expected = new HistoryGraphItem<string>(
                new HistoryVertex(1),
                new[] { new HistoryEdge(0, 0) },
                new[] { new HistoryEdge(0, 0), new HistoryEdge(1, 1) },
                new[] { "3", "2" });

            var actual = BranchFromVertexGraphRenderer.Render(commit, incomings);

            Assert.Equal(expected.Vertex, actual.Vertex);
            Assert.Equal(expected.InboundEdges, actual.InboundEdges);
            Assert.Equal(expected.OutboundEdges, actual.OutboundEdges);
            Assert.Equal(expected.OutboundVertexes, actual.OutboundVertexes);
        }
    }
}
