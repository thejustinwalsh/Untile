using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Untile
{
    class Program
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Required]
        [FileExists]
        [Option(Description = "The input image to process")]
        public string Input { get; }

        [LegalFilePath]
        [Option(Description = "The output path to extract the images to, if no path is given defaults to working directory")]
        public string Output { get; }

        [Required]
        [TileSize]
        [Option(Description = "The size each tile. i.e. 16x16")]
        public string Tiles { get; }

        private void OnExecute()
        {
            var tileWidth = Int32.Parse(Tiles.Split("x")[0]);
            var tileHeight = Int32.Parse(Tiles.Split("x")[1]);

            var rect = new SixLabors.Primitives.Rectangle(0, 0, tileWidth, tileHeight);
            using (var img = Image.Load(Input))
            {
                var index = 0;
                var stepX = img.Width / tileWidth;
                var stepY = img.Height / tileHeight;

                var dir = Output ?? System.Environment.CurrentDirectory;

                for (int y = 0; y < stepY; y++)
                {
                    for (int x = 0; x < stepX; x++)
                    {
                        rect.X = x * tileWidth;
                        rect.Y = y * tileHeight;

                        using (var sub = img.Clone(ctx => ctx.Crop(rect)))
                        {
                            sub.Save(System.IO.Path.Combine(dir, $"image-{index}.png"));
                        }

                        index++;
                    }
                }
            }
        }
    }

    class TileSizeAttribute : ValidationAttribute
    {
        public TileSizeAttribute()
            : base("The value for {0} must be in the format of [width]x[height] i.e 16x16")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value != null && value is string)
            {
                var values = (value as string).Split("x");
                if (values.Length == 2 && Int32.TryParse(values[0], out int resX) && Int32.TryParse(values[1], out int resY))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(context.DisplayName));
        }
    }
}
