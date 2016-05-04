using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Encommit.Views.HistoryGraph
{
    static class HistoryGraphGeometries
    {
        internal static readonly double Height = 24;

        internal static readonly double StrokeTickness = 2;
        internal static readonly SolidColorBrush Stroke = new SolidColorBrush(Colors.DarkGray);
        internal static readonly SolidColorBrush Fill = new SolidColorBrush(Colors.DarkGray);

        internal static readonly double XGap = 12;
        internal static readonly double XStart = XGap / 2;

        internal static readonly double YInboudStart = 0;
        internal static readonly double YInboudEnd = Height / 2;
        internal static readonly double YOutboudStart = Height / 2;
        internal static readonly double YOutboudEnd = Height;

        internal static readonly double VertexSize = 8;

        internal static double XPositoinFromIndex(int index)
        {
            return index * XGap + XStart;
        }

        internal static double XVertexPosition(int index)
        {
            return XPositoinFromIndex(index) - VertexSize / 2;
        }

        internal static readonly double YVertexPosition = (Height - VertexSize) / 2;
    }
}
