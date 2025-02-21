namespace Skeletom.Essentials.Utils {

    public static class MathUtils {
        public static float Normalize(float val, float valmin, float valmax, float min, float max){
            float numerator = (val - valmin) / (valmax - valmin);
            numerator = double.IsNaN(numerator) ? 0f : numerator;
            return numerator == 0f ? min : (numerator * (max - min)) + min;
        }
    }
}