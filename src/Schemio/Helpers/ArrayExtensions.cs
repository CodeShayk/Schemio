namespace Schemio.Helpers
{
    public static class ArrayExtensions
    {

        public static T[] EnsureAndResizeArray<T>(this T[] val, out int index)
        {
            return val.EnsureAndResizeArray(1, out index);
        }

        public static T[] EnsureAndResizeArray<T>(this T[] val, int extendBy, out int index)
        {
            if (val == null)
            {
                val = new T[extendBy];
                index = 0;
            }
            else
            {
                var temp = val;
                index = temp.Length;
                Array.Resize(ref temp, temp.Length + extendBy);
                val = temp;
            }

            return val;
        }
    }
}