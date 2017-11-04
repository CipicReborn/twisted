public class Utils {

    public static void LoopValue (ref int value, int min, int max) {
        if (value == max + 1) {
            value = min;
        }
        else if (value == min - 1) {
            value = max;
        }
    }
}
