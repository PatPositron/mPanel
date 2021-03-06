﻿using System;
using System.Drawing;
using System.Windows.Forms;
using mPanel.Matrix;

namespace mPanel.Controls
{
    public sealed class PanelPreview : Control
    {
        private static int PixelDataLength => MatrixPanel.PixelDataLength;
        private readonly byte[] FrameBuffer;

        public int PanelWidth => MatrixPanel.Width;
        public int PanelHeight => MatrixPanel.Height;
        public int PixelSize { get; set; }
        public int GapSize { get; set; }

        public PanelPreview()
        {
            DoubleBuffered = true;

            FrameBuffer = new byte[PanelWidth * PanelHeight * PixelDataLength];
        }

        public void UpdatePreview(byte[] data)
        {
            if (data?.Length != PanelWidth * PanelHeight * 3)
                return;

            Array.Copy(data, 0, FrameBuffer, 0, data.Length);
            Invalidate();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            if (DesignMode)
                return;

            var size = Math.Min(Width, Height);
            var dim = Math.Max(PanelWidth, PanelHeight);

            PixelSize = (size - (dim - 1) * GapSize) / dim;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);

            var index = 0;
            var x = 0;
            var y = 0;

            for (var i = 0; i < FrameBuffer.Length; i += PixelDataLength)
            {
                var color = Color.FromArgb(FrameBuffer[i], FrameBuffer[i + 1], FrameBuffer[i + 2]);
                
                using (var brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, x, y, PixelSize, PixelSize);
                }

                g.DrawRectangle(Pens.Black, x, y, PixelSize - 1, PixelSize - 1);

                x += PixelSize + GapSize;
                index++;

                if (index % PanelWidth != 0)
                    continue;

                x = 0;
                y += PixelSize + GapSize;
            }
        }
    }
}
