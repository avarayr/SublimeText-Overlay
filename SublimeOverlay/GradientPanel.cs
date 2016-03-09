using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class GradientPanel : Panel
{
    public Color GradientFirstColor = Color.FromArgb(33, 33, 33);
    public Color GradientSecondColor = Color.FromArgb(22, 22, 22);
    public GradientPanel()
    {
        this.ResizeRedraw = true;
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            return;
        using (var brush = new LinearGradientBrush(ClientRectangle,
                   GradientFirstColor, GradientSecondColor, LinearGradientMode.Vertical))
        {
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }
    }
    protected override void OnScroll(ScrollEventArgs se)
    {
        this.Invalidate();
        base.OnScroll(se);
    }
}