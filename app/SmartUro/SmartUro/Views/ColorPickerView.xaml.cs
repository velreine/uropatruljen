using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SmartUro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPickerView : ContentPage
    {
        public ColorPickerView()
        {
            InitializeComponent();
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            var scale = 21F;
            SKPath path = new SKPath();
            path.MoveTo(-1 * scale, -1 * scale);
            path.LineTo(0 * scale, -1 * scale);
            path.LineTo(0 * scale, 0 * scale);
            path.LineTo(1 * scale, 0 * scale);
            path.LineTo(1 * scale, 1 * scale);
            path.LineTo(0 * scale, 1 * scale);
            path.LineTo(0 * scale, 0 * scale);
            path.LineTo(-1 * scale, 0 * scale);
            path.LineTo(-1 * scale, -1 * scale);

            SKMatrix matrix = SKMatrix.MakeScale(2 * scale, 2 * scale);
            SKPaint paint = new SKPaint
            {
                PathEffect = SKPathEffect.Create2DPath(matrix, path),
                Color = Color.LightGray.ToSKColor(),
                IsAntialias = true
            };

            var patternRect = new SKRect(0, 0, ((SKCanvasView)sender).CanvasSize.Width, ((SKCanvasView)sender).CanvasSize.Height);

            canvas.Save();
            canvas.DrawRect(patternRect, paint);
            canvas.Restore();
        }
    }
}