using Godot;

namespace Lyrq.utils.cs;

public static class Extensions
{
    // Метод для ограничения значений Vector2 в заданном диапазоне
    public static Vector2 CoerceIn(this Vector2 vector, float min, float max)
    {
        return new Vector2(
            Mathf.Clamp(vector.X, min, max),
            Mathf.Clamp(vector.Y, min, max)
        );
    }


    // Метод для добавления сразу ко всем направлениям Vector2
    public static Vector2 Plus(this Vector2 vector, float toAdd)
    {
        return new Vector2(
            vector.X + toAdd,
            vector.Y + toAdd
        );
    }
    
    // Если значение меньше minimum, возвращаем minimum, иначе возвращаем value
    public static float CoerceAtLeast(this float value, float minimum) => value < minimum ? minimum : value;
    
}