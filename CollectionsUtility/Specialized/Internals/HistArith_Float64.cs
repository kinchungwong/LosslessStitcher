namespace CollectionsUtility.Specialized.Internals
{
    public struct HistArith_Float64
        : IHistArith<double>
    {
        public double Zero => 0.0;

        public double One => 1.0;

        public double Add(double arg0, double arg1)
        {
            return arg0 + arg1;
        }

        public int Compare(double arg0, double arg1)
        {
            return (arg0 > arg1) ? 1 : (arg0 < arg1) ? -1 : 0;
        }
    }
}
