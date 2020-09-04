namespace LosslessStitcher.Imaging.Hash2D.Simple
{
    public class StageInfo
        : IStageInfo
    {
        public Direction Direction { get; }

        public int Radius { get; }

        public int Spacing { get; }

        public StageInfo(Direction dir, int radius, int spacing)
        {
            Direction = dir;
            Radius = radius;
            Spacing = spacing;
        }

        public StageInfo(IStageInfo other)
        {
            Direction = other.Direction;
            Radius = other.Radius;
            Spacing = other.Spacing;
        }
    }
}
