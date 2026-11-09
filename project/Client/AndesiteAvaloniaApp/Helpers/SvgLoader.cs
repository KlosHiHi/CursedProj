using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using SkiaSharp;
using Svg.Skia;

using System;
using System.IO;

namespace AndesiteAvaloniaApp.Helpers;

public class SvgImage : Control
{
    public static readonly StyledProperty<string?> SourceProperty =
        AvaloniaProperty.Register<SvgImage, string?>(
            nameof(Source));

    private Bitmap? _bitmap;

    public string? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    static SvgImage()
    {
        AffectsRender<SvgImage>(SourceProperty);
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SourceProperty)
        {
            LoadSvg();
        }
    }

    private void LoadSvg()
    {
        _bitmap?.Dispose();
        _bitmap = null;

        if (string.IsNullOrWhiteSpace(Source))
        {
            InvalidateVisual();
            return;
        }

        try
        {
            using var stream = OpenSource(Source);

            var svg = new SKSvg();
            svg.Load(stream);

            var picture = svg.Picture;

            if (picture == null)
                return;

            var bounds = picture.CullRect;

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            const int scale = 3;

            var width = (int)Math.Ceiling(bounds.Width * scale);
            var height = (int)Math.Ceiling(bounds.Height * scale);

            using var skBitmap = new SKBitmap(width, height);

            using (var canvas = new SKCanvas(skBitmap))
            {
                canvas.Clear(SKColors.Transparent);

                canvas.Scale(scale);
                canvas.Translate(-bounds.Left, -bounds.Top);

                canvas.DrawPicture(picture);
            }

            using var image = SKImage.FromBitmap(skBitmap);
            using var data = image.Encode(
                SKEncodedImageFormat.Png, 100);

            using var pngStream = data.AsStream();

            _bitmap = new Bitmap(pngStream);

            InvalidateVisual();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SVG loading error: {ex.Message}");
        }
    }

    private static Stream OpenSource(string source)
    {
        if (source.StartsWith("avares://",
                StringComparison.OrdinalIgnoreCase))
        {
            return AssetLoader.Open(new Uri(source));
        }

        return File.OpenRead(source);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (_bitmap == null)
            return;

        var imageWidth = _bitmap.PixelSize.Width;
        var imageHeight = _bitmap.PixelSize.Height;

        var scale = Math.Min(
            Bounds.Width / imageWidth,
            Bounds.Height / imageHeight);

        var width = imageWidth * scale;
        var height = imageHeight * scale;

        var x = (Bounds.Width - width) / 2;
        var y = (Bounds.Height - height) / 2;

        var destination = new Rect(x, y, width, height);

        var sourceRect = new Rect(
            0, 0, imageWidth, imageHeight);

        context.DrawImage(
            _bitmap,
            sourceRect,
            destination);
    }

    protected override void OnDetachedFromVisualTree(
        VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _bitmap?.Dispose();
        _bitmap = null;
    }
}