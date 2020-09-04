namespace LosslessStitcher.Imaging.Hash2D
{
    public interface IStageInfo
    {
        /// <summary>
        /// When computing the output value for the current pixel coordinate, <see cref="Direction"/> specifies
        /// whether the input pixels will be horizontally or vertically relative to the current pixel coordinates.
        /// </summary>
        Direction Direction { get; }

        /// <summary>
        /// When computing the output value for the current pixel coordinate, the hash function will consume 
        /// this number of input pixels on both the left side and the right side of the current pixel coordinate.
        /// 
        /// <para>
        /// The total number of input pixels consumed is given by: 
        /// <br/>
        /// <c>(2 * WindowRadius + 1)</c>
        /// </para>
        /// 
        /// <para>
        /// The input pixels consumed are not necessarily contiguous. Refer to the <see cref="Spacing"/> parameter.
        /// </para>
        /// </summary>
        int Radius { get; }

        /// <summary>
        /// When the hash function computes the input pixel coordinates in order to compute the output value for the 
        /// current pixel coordinate, this parameter specifies the horizontal or vertical coordinate increment.
        /// 
        /// <para>
        /// The minimum valid value is <c>1</c>. When this minimum value is used, contiguous input pixels are 
        /// used to compute each output pixel.
        /// </para>
        /// </summary>
        int Spacing { get; }
    }
}
