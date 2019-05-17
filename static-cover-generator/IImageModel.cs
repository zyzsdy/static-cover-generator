namespace static_cover_generator
{
    internal interface IImageModel
    {
        void Init(string input, string output, int width, int height);
        void Process();
    }
}