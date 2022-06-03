﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPickerControl : ContentView
    {
        private SKPoint _lastTouchPoint = new SKPoint();

        public event EventHandler<Color> PickedColorChanged;

        public static readonly BindableProperty PickedColorProperty
            = BindableProperty.Create(
                nameof(PickedColor),
                typeof(Color),
                typeof(ColorPickerControl));

        public Color PickedColor
        {
            get { return (Color)GetValue(PickedColorProperty); }
            set { SetValue(PickedColorProperty, value); }
        }
        public ColorPickerControl()
        {
            InitializeComponent();
        }

        private void SkCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var skImageInfo = e.Info;
            var skSurface = e.Surface;
            var skCanvas = skSurface.Canvas;

            var skCanvasWidth = skImageInfo.Width;
            var skCanvasHeight = skImageInfo.Height;

            skCanvas.Clear(SKColors.White);

            // Draw gradient rainbow Color spectrum
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;

                // Initiate the primary Color list
                // picked up from Google Web Color Picker
                var colors = new SKColor[]
                {
                new SKColor(255, 0, 0), // Red
                new SKColor(255, 255, 0), // Yellow
                new SKColor(0, 255, 0), // Green (Lime)
                new SKColor(0, 255, 255), // Aqua
                new SKColor(0, 0, 255), // Blue
                new SKColor(255, 0, 255), // Fuchsia
                new SKColor(255, 0, 0), // Red
                };

                // create the gradient shader between Colors
                using (var shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(skCanvasWidth, 0),
                    colors,
                    null,
                    SKShaderTileMode.Clamp))
                {
                    paint.Shader = shader;
                    skCanvas.DrawPaint(paint);
                }
            }
            // Draw darker gradient spectrum
            using (var brightnessPaint = new SKPaint())
            {
                brightnessPaint.IsAntialias = true;

                // Initiate the darkened primary color list
                var colors = new SKColor[]
                {
                    SKColors.White,
                    SKColors.Transparent,
                    SKColors.Black
                };

                // create the gradient shader 
                using (var shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(0, skCanvasHeight),
                    colors,
                    null,
                    SKShaderTileMode.Clamp))
                {
                    brightnessPaint.Shader = shader;
                    skCanvas.DrawPaint(brightnessPaint);
                }
            }

            // Picking the Pixel Color values on the Touch Point

            // Represent the color of the current Touch point
            SKColor touchPointColor;

            // Efficient and fast
            // https://forums.xamarin.com/discussion/92899/read-a-pixel-info-from-a-canvas
            // create the 1x1 bitmap (auto allocates the pixel buffer)
            using (SKBitmap bitmap = new SKBitmap(skImageInfo))
            {
                // get the pixel buffer for the bitmap
                IntPtr dstpixels = bitmap.GetPixels();

                // read the surface into the bitmap
                skSurface.ReadPixels(skImageInfo,
                    dstpixels,
                    skImageInfo.RowBytes,
                    (int)_lastTouchPoint.X, (int)_lastTouchPoint.Y);

                // access the color
                touchPointColor = bitmap.GetPixel(0, 0);
            }

            // Painting the Touch point
            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Fill;
                paintTouchPoint.Color = SKColors.White;
                paintTouchPoint.IsAntialias = true;

                // Outer circle (Ring)
                var outerRingRadius =
                    ((float)skCanvasWidth /
                        (float)skCanvasHeight) * (float)18;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    outerRingRadius, paintTouchPoint);

                // Draw another circle with picked color
                paintTouchPoint.Color = touchPointColor;

                // Outer circle (Ring)
                var innerRingRadius =
                    ((float)skCanvasWidth /
                        (float)skCanvasHeight) * (float)12;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    innerRingRadius, paintTouchPoint);
            }

            // Set selected color
            PickedColor = touchPointColor.ToFormsColor();
            PickedColorChanged?.Invoke(this, PickedColor);
        }

        private void SkCanvasView_OnTouch(object sender, SKTouchEventArgs e)
        {
            _lastTouchPoint = e.Location;

            var canvasSize = SkCanvasView.CanvasSize;

            // Check for each touch point XY position to be inside Canvas
            // Ignore any Touch event ocurred outside the Canvas region 
            if ((e.Location.X > 0 && e.Location.X < canvasSize.Width) &&
                (e.Location.Y > 0 && e.Location.Y < canvasSize.Height))
            {
                e.Handled = true;

                // update the Canvas as you wish
                SkCanvasView.InvalidateSurface();
            }
        }
    }
}