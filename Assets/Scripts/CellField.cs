using UnityEngine;

public class CellField : MonoBehaviour
{
    //   2
    // 1 x 4
    //   8

    private bool[] walls;
    public bool[] Walls
    {
        get
        {
            return walls;
        }
    }
    
    // Информация о стенках клетки.
    public int Index
    {
        get
        {
            int res = 0;

            for (int i = 0; i < walls.Length; i++)
            {
                res += (int)Mathf.Pow(2, i) * (walls[i] ? 1 : 0);
            }

            return res;
        }
    }

    // Информация о присутствии знака на клетке.
    public bool signed { get; set;}

    // Координаты для нахождения соседа и успешной постановки стеночек.
    public int x { get; private set; }
    public int y { get; private set; }
    
    // Задаем массиву walls стандартные значения (false).
    private void Start()
    {
        walls = new bool[4];
    }

    /// <summary>
    /// Инициализирует координаты клетки.
    /// </summary>
    /// <param name="i">координата x</param>
    /// <param name="j">координата y</param>
    public void InitializeCoord(int i, int j)
    {        
        x = i;
        y = j;
    }

    /// <summary>
    /// Добавляет стенку (если она неактивна) и меняет спрайт клетки.
    /// </summary>
    /// <param name="i">координата стенки</param>
    /// <param name="sign">нужно ли ставить знак в клетку</param>
    public void AddSprite(int i, bool sign)
    {
        if (i >= 0)
        {
            // Добавляем стенку.
            walls[i] = true; 
        }
        if (!sign)
        {
            // Если ставить знак не надо, то ставим стенку.
            SetSprite(GameManager.instance.sprites[Index]);
        }
        else
        {
            signed = true;

            // Ставим знак в клетку.
            if (Player.player.CrossTurn)
            {
                SetSprite(GameManager.instance.sprites[16]);
                Player.player.CrossCount++; 
            }
            else
            {
                SetSprite(GameManager.instance.sprites[17]);
                Player.player.CircleCount++;
            }
        }
    }

    /// <summary>
    /// Задает спрайт данной клетке поля.
    /// </summary>
    private void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Нажатие на клетку поля.
    private void OnMouseDown()
    {
        // Запускаем функцию нажатия клетки на поле из игрового менеджера и передаем в нее данную клетку.
        GameManager.instance.FieldCellPressed(gameObject);
    }
}
