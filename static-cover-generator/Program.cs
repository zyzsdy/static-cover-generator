using CommandLine;
using System;
using System.Linq;

namespace static_cover_generator
{
    class Options
    {
        [Option('i', "input", MetaValue = "FILE", Required = true, HelpText = "Input image")]
        public string InputImage { get; set; }

        [Option('o', "output", MetaValue = "FILE", Required = true, HelpText = "Output file")]
        public string OutputFile { get; set; }

        [Option('m', "model", Default = "default", Required = false, HelpText = "Image generation model")]
        public string Model { get; set; }

        [Option('w', "width", Default = 1920, Required = false, HelpText = "Width of output image")]
        public int Width { get; set; }

        [Option('h', "height", Default = 1080, Required = false, HelpText = "Height of output image")]
        public int Height { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (string.IsNullOrEmpty(o.Model))
                {
                    Console.Error.WriteLine("Model name is empty?????");
                    Environment.ExitCode = 1;
                    return;
                }

                string modelName = "static_cover_generator.models." + o.Model.First().ToString().ToUpper() + o.Model.Substring(1) + "Model";
                Type modelType = Type.GetType(modelName);

                if (modelType == null)
                {
                    Console.Error.WriteLine("Couldn't find model " + modelName);
                    Environment.ExitCode = 2;
                    return;
                }

                IImageModel im = (IImageModel)Activator.CreateInstance(modelType);

                im.Init(o.InputImage, o.OutputFile, o.Width, o.Height);
                im.Process();
            });
        }
    }
}
