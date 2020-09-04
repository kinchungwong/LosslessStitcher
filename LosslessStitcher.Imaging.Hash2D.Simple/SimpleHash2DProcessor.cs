using System;
using System.Collections.Generic;
using HashCodeUtility;

namespace LosslessStitcher.Imaging.Hash2D.Simple
{
    using Data;

    public class SimpleHash2DProcessor
        : IHash2DProcessor
    {
        public IBitmapFactory BitmapFactory { get; set; }

        private StageList _stages;

        public IStageList Stages => _stages;

        public SimpleHash2DProcessor(IBitmapFactory bitmapFactory)
        {
            if (bitmapFactory is null)
            {
                throw new ArgumentNullException(nameof(bitmapFactory));
            }
            BitmapFactory = bitmapFactory;
            _stages = new StageList();
        }

        public void AddStage(Direction dir, int radius, int spacing)
        {
            _stages._list.Add(new StageInfo(dir, radius, spacing));
        }

        public void AddStage(IStageInfo stage)
        {
            _stages._list.Add(new StageInfo(stage));
        }

        public void AddStages(IStageList stageList)
        {
            foreach (var stage in stageList)
            {
                _stages._list.Add(new StageInfo(stage));
            }
        }

        public void Process(IArrayBitmap<int> input, IArrayBitmap<int> output)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            Size size = input.Size;
            if (!size.Equals(output.Size))
            {
                throw new ArgumentException(nameof(output));
            }
            _ProcessStages(new StageList(_stages), input, output);
        }

        private void _ProcessStages(StageList stages, IArrayBitmap<int> input, IArrayBitmap<int> output)
        {
            Size size = input.Size;
            Rect prevStageRect = new Rect(Point.Origin, size);
            IArrayBitmap<int> prevStageOutput = null;
            int stageCount = Stages.Count;
            for (int stageIndex = 0; stageIndex < stageCount; ++stageIndex)
            {
                var stageInfo = stages[stageIndex];
                Rect stageOutputRect = _ComputeOutputRect(prevStageRect, stageInfo);
                bool isFirst = stageIndex == 0;
                bool isLast = stageIndex + 1 == stageCount;
                IArrayBitmap<int> stageInput = isFirst ? input : prevStageOutput;
                IArrayBitmap<int> stageOutput = isLast ? output : _RentBitmap(size);
                _ProcessStage(stageInfo, stageInput, stageOutput, stageOutputRect);
                if (!(prevStageOutput is null))
                {
                    _ReturnBitmap(prevStageOutput);
                }
                prevStageOutput = stageOutput;
                prevStageRect = stageOutputRect;
            }
        }

        private void _ProcessStage(StageInfo stage, IArrayBitmap<int> input, IArrayBitmap<int> output, Rect outputRect)
        {
            switch (stage.Direction)
            {
                case Direction.Horz:
                    _ProcessStage_Horz(stage, input, output, outputRect);
                    return;
                case Direction.Vert:
                    _ProcessStage_Vert(stage, input, output, outputRect);
                    return;
                default:
                    throw new NotImplementedException();
            }
        }

        private void _ProcessStage_Horz(StageInfo stage, IArrayBitmap<int> input, IArrayBitmap<int> output, Rect outputRect)
        {
            int accessRadius = stage.Radius * stage.Spacing;
            for (int outY = outputRect.Top; outY < outputRect.Bottom; ++outY)
            {
                for (int outX = outputRect.Left; outX < outputRect.Right; ++outX)
                {
                    HashCodeBuilder hcb = new HashCodeBuilder(0u);
                    for (int deltaX = -accessRadius; deltaX <= +accessRadius; deltaX += stage.Spacing)
                    {
                        hcb.Ingest(input[outX + deltaX, outY]);
                    }
                    output[outX, outY] = hcb.GetHashCode();
                }
            }
        }

        private void _ProcessStage_Vert(StageInfo stage, IArrayBitmap<int> input, IArrayBitmap<int> output, Rect outputRect)
        {
            int accessRadius = stage.Radius * stage.Spacing;
            for (int outY = outputRect.Top; outY < outputRect.Bottom; ++outY)
            {
                for (int outX = outputRect.Left; outX < outputRect.Right; ++outX)
                {
                    HashCodeBuilder hcb = new HashCodeBuilder(0u);
                    for (int deltaY = -accessRadius; deltaY <= +accessRadius; deltaY += stage.Spacing)
                    {
                        hcb.Ingest(input[outX, outY + deltaY]);
                    }
                    output[outX, outY] = hcb.GetHashCode();
                }
            }
        }

        private IArrayBitmap<int> _RentBitmap(Size size)
        {
            return BitmapFactory.Create<int>(size.Width, size.Height);
        }

        private void _ReturnBitmap(IArrayBitmap<int> bitmap)
        {
            // ======
            // Do nothing (by intention)
            // ======
        }

        private Rect _ComputeOutputRect(Rect inputRect, StageInfo stage)
        {
            int accessRadius = stage.Radius * stage.Spacing;
            switch (stage.Direction)
            {
                case Direction.Horz:
                    return new Rect(
                        inputRect.X + accessRadius,
                        inputRect.Y,
                        inputRect.Width - 2 * accessRadius,
                        inputRect.Height);
                case Direction.Vert:
                    return new Rect(
                        inputRect.X,
                        inputRect.Y + accessRadius,
                        inputRect.Width,
                        inputRect.Height - 2 * accessRadius);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
