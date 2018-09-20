using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    // Префаб для одиночной клеточки на поле.
    public GameObject cell;

    // Родитель всех клеток.
    public Transform cellsHandler;

    // Максимальные длины сторон игрового поля.
    private const int maxWidth = 10, maxHeight = 6;

    // Максимальные длины сторон вырезанных прямоугольников.
    private const int maxRectW = 4, maxRectH = 3; 

    /// <summary>
    /// Заполняет массив array рандомными значениями.
    /// </summary>
    /// <param name="array">массив</param>
    /// <param name="maxValue">максимальное значение для значений массива (не включается)</param>
    private static void SetRandomValues(ref int[] array, int maxValue)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Random.Range(1, maxValue);
        }
    }
    
    /// <summary>
    /// Проверяет, лежит ли клетка в активном поле.
    /// </summary>
    /// <param name="j">координата x клетки</param>
    /// <param name="i">координата y клетки</param>
    /// <param name="width">массив, задающий ширину вырезанных прямоугольников</param>
    /// <param name="height">массив, задающий высоту вырезанных прямоугольников</param>
    /// <returns>true - клетка лежит в активном поле, иначе - false</returns>
    private static bool CheckActive(int j, int i, int[] width, int[] height)
    {
        // Лежит ли клетка в неактивном поле (в вырезанных прямоугльниках).
        bool result = false;

        // Проверка по прямоугольникам.
        result |= j >= 0 && j < width[0] && i >= 0 && i < height[0];
        result |= j >= maxWidth - width[1] && i >= 0 && i < height[1];
        result |= j >= 0 && j < width[2] && i >= maxHeight - height[2]; 
        result |= j >= maxWidth - width[3] && i >= maxHeight - height[3];

        return !result;
    }
    
    /// <summary>
    /// Генератор игрового поля.
    /// </summary>
    public void GenerateField()
    {
        // Размеры вырезанных прямоугольников.
        int[] width = new int[4];
        int[] height = new int[4];

        // Заполняем массивы рандомными значениями.
        SetRandomValues(ref width, maxRectW);
        SetRandomValues(ref height, maxRectH);

        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                if (CheckActive(j, i, width, height))
                {
                    GameObject activeCell = Instantiate(cell, new Vector3(j, i, -4f), Quaternion.identity);
                    activeCell.transform.SetParent(cellsHandler);
                    activeCell.GetComponent<CellField>().InitializeCoord(j, i);
                }
            }
        }
    }
}
