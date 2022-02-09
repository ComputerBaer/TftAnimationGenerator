namespace TftAnimationGenerator.Models
{
    public class ExportQueueEntry
    {
        public string Name { get; init; }
        public string Filename { get; init; }

        public int Width { get; init; }
        public int Height { get; init; }

        public ExportQueueEntry Clone()
        {
            return new()
            {
                Name = Name,
                Filename = Filename,
                Width = Width,
                Height = Height,
            };
        }
    }
}
