using ImageMagick;

namespace static_cover_generator.models
{
    class DefaultModel : IImageModel
    {
        string input;
        string output;
        int width;
        int height;

        public void Init(string input, string output, int width, int height)
        {
            this.input = input;
            this.output = output;
            this.width = width;
            this.height = height;
        }

        public void Process()
        {
            using (var bg = new MagickImage(MagickColor.FromRgba(255, 255, 255, 255), width, height))
            {
                using (var image = new MagickImage(input))
                {
                    if (image.HasAlpha == false)
                    {
                        image.Alpha(AlphaOption.Opaque);
                    }
                    using (var cover = image.Clone())
                    {
                        MagickGeometry resize = new MagickGeometry(width + "x" + height + "^");
                        image.Resize(resize);
                        image.Extent(width, height, Gravity.Center);
                        image.Blur(80, 30);

                        bg.Composite(image, 0, 0, CompositeOperator.Over);

                        MagickGeometry centerResize = new MagickGeometry(height * 0.7 + "x" + height * 0.7);
                        cover.Resize(centerResize);
                        using (var coverShadow = cover.Clone())
                        {
                            coverShadow.BackgroundColor = MagickColor.FromRgba(192, 192, 192, 192);
                            coverShadow.Shadow(50, 50, 10, new Percentage(90));

                            bg.Composite(coverShadow, Gravity.Center, CompositeOperator.Over);
                        }

                        bg.Composite(cover, Gravity.Center, CompositeOperator.Over);
                    }
                }
                bg.Write(output);
            }
        }
    }
}
