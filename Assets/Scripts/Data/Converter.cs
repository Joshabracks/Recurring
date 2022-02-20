namespace Gameplay.Data {
    public class Converter
    {
        public int BinaryToInt(int[] binary) {
            int result = 0;
            int iter = 1;
            for (int i = binary.Length - 1; i >= 0; i--) {
                result += iter * binary[i];
                iter *= 2;
            }
            return result;
        }
    }
}
