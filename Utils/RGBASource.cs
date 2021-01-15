using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Inferno_Mod_Manager.Utils
{
    class RGBASource : BitmapSource
    {
        private byte[] rgbaBuffer;
        private int pixelWidth;
        private int pixelHeight;

        public RGBASource(byte[] rgbaBuffer, int pixelWidth)
        {
            this.rgbaBuffer = rgbaBuffer;
            this.pixelWidth = pixelWidth;
            this.pixelHeight = rgbaBuffer.Length / (4 * pixelWidth);
        }

        public override void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset) {
            for (var y = sourceRect.Y; y < sourceRect.Y + sourceRect.Height; y++)
            for (var x = sourceRect.X; x < sourceRect.X + sourceRect.Width; x++)
            {
                var i = stride * y + 4 * x;
                var a = rgbaBuffer[i + 3];
                var r = (byte) (rgbaBuffer[i] * a / 256); // pre-multiplied R
                var g = (byte) (rgbaBuffer[i + 1] * a / 256); // pre-multiplied G
                var b = (byte) (rgbaBuffer[i + 2] * a / 256); // pre-multiplied B

                pixels.SetValue(b, i + offset);
                pixels.SetValue(g, i + offset + 1);
                pixels.SetValue(r, i + offset + 2);
                pixels.SetValue(a, i + offset + 3);
            }
        }

        protected override Freezable CreateInstanceCore() => new RGBASource(rgbaBuffer, pixelWidth);

#pragma warning disable CS0067
        public override event EventHandler<DownloadProgressEventArgs> DownloadProgress;
        public override event EventHandler DownloadCompleted;
        public override event EventHandler<ExceptionEventArgs> DownloadFailed;
        public override event EventHandler<ExceptionEventArgs> DecodeFailed;
#pragma warning restore CS0067

        public override double DpiX => 96;

        public override double DpiY => 96;

        public override PixelFormat Format => PixelFormats.Pbgra32;

        public override int PixelWidth => pixelWidth;

        public override int PixelHeight => pixelHeight;

        public override double Width => pixelWidth;

        public override double Height => pixelHeight;
    }
}