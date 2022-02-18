namespace Gameplay.Data
{
    public struct Array3D<T>
    {
        public int Width { get; }
        public int Depth { get; }
        public int Height { get; }
        private T[] data;
        public Array3D(int width, int depth, int height)
        {
            Width = width;
            Depth = depth;
            Height = height;
            data = new T[Width * Depth * Height];
        }
        public void Set(int x, int y, int z, T value)
        {
            data[index(x, y, z)] = value;
        }
        public T Get(int x, int y, int z)
        {
            return data[index(x, y, z)];
        }
        private int index(int x, int y, int z)
        {
            return (Height * Depth * x) + (Height * y) + z;
        }

        public T[] Raw()
        {
            return data;
        }

        public int Length()
        {
            return data.Length;
        }
    }

    public struct Array2D<T>
    {
        public int Width { get; }
        public int Height { get; }
        private T[] data;
        public Array2D(int width, int height)
        {
            Width = width;
            Height = height;
            data = new T[Width * Height];
        }

        public Array2D(int width, int height, T[] arrayData)
        {
            Width = width;
            Height = height;
            data = arrayData;
        }

        public Array2D(int size, T[] arrayData)
        {
            Width = size;
            Height = size;
            data = arrayData;
        }
        public Array2D(int size)
        {
            Width = size;
            Height = size;
            data = new T[Width * Height];
        }
        public void Set(int x, int y, T value)
        {
            data[index(x, y)] = value;
        }
        public T Get(int x, int y)
        {
            return data[index(x, y)];
        }
        public T[] Raw()
        {
            return data;
        }
        private int index(int x, int y)
        {
            return (x * Width) + y;
        }

        public int Length()
        {
            return data.Length;
        }
    }

}