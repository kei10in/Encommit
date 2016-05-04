using Encommit.Models.HistoryGraph;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Media;

using static Encommit.Views.HistoryGraph.HistoryGraphGeometries;

namespace Encommit.Views.HistoryGraph
{
    class HistoryGraphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var graph = value as HistoryGraphItem<ObjectId>;
            if (graph == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var result = new List<UIElement>();

            foreach (var c in graph.InboundEdges)
            {
                result.Add(EdgeShape(c, true));
            }

            foreach (var c in graph.OutboundEdges)
            {
                result.Add(EdgeShape(c, false));
            }

            result.Add(VertexEllipse(graph.Vertex));

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Shape EdgeShape(HistoryEdge edge, bool isInbound)
        {
            var x1 = XPositoinFromIndex(edge.From);
            var y1 = isInbound ? YInboudStart : YOutboudStart;

            var x2 = XPositoinFromIndex(edge.To);
            var y2 = isInbound ? YInboudEnd : YOutboudEnd;

            if (edge.From == edge.To)
            {
                return EdgeLine(x1, y1, x2, y2);
            }
            else
            {
                return EdgeBezier(x1, y1, x2, y2);
            }
        }

        private Line EdgeLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = StrokeTickness;
            line.Stroke = Stroke;

            return line;
        }

        private Path EdgeBezier(double x1, double y1, double x2, double y2)
        {
            var geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(x1, y1), false, false);
                context.QuadraticBezierTo(
                    new Point(x1, y1 + (y2 - y1) / 3),
                    new Point((x1 + x2) / 2, (y1 + y2) / 2),
                    true, true);
                context.QuadraticBezierTo(
                    new Point(x2, y2 - (y2 - y1) / 3),
                    new Point(x2, y2),
                    true, true);
            }
            geometry.Freeze();

            var path = new Path();
            path.StrokeThickness = StrokeTickness;
            path.Stroke = Stroke;
            path.Data = geometry;

            return path;
        }

        private Shape VertexEllipse(HistoryVertex vertex)
        {
            var mark = new Ellipse();
            var x = XPositoinFromIndex(vertex.Index);
            Canvas.SetLeft(mark, XVertexPosition(vertex.Index));
            Canvas.SetTop(mark, YVertexPosition);
            mark.Width = VertexSize;
            mark.Height = VertexSize;
            mark.Fill = Fill;
            return mark;
        }
    }
}
