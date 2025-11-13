using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public static class Theme
{
    public static void AplicarGradiente(Form form, Color cor1, Color cor2)
    {
        form.BackgroundImage = CriarGradiente(form.ClientSize, cor1, cor2);
        form.BackgroundImageLayout = ImageLayout.Stretch;

        form.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        };
    }

    private static Bitmap CriarGradiente(Size size, Color cor1, Color cor2)
    {
        Bitmap bmp = new Bitmap(size.Width, size.Height);
        using (Graphics g = Graphics.FromImage(bmp))
        using (LinearGradientBrush brush = new LinearGradientBrush(
                 new Rectangle(0, 0, size.Width, size.Height),
                 cor1, cor2,
                 45f))
        {
            g.FillRectangle(brush, 0, 0, size.Width, size.Height);
        }
        return bmp;
    }
}
