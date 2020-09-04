namespace LosslessStitcher.Imaging.Hash2D
{
    public interface IHash2DProcessor
    {
        IBitmapFactory BitmapFactory { get; set; }

        IStageList Stages { get; }

        void AddStage(Direction dir, int radius, int spacing);

        void AddStage(IStageInfo stage);

        void AddStages(IStageList stageList);

        void Process(IArrayBitmap<int> input, IArrayBitmap<int> output);
    }
}
