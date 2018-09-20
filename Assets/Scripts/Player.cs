using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Ссылка на действующую версию компнента.
    public static Player player;
    
    // Количество крестиков и ноликов.
    public Text crossNumbText, circleNumbText;

    // Изображения для счета знаков.
    public Image cross, circle;

    // Выделение счета знаков.
    public GameObject crossHighlight, circleHighlight;
        
    private bool crossTurn;
    /// <summary>
    /// Очередь крестиков.
    /// </summary>
    public bool CrossTurn
    {
        get
        {
            return crossTurn;
        }
        set
        {
            crossTurn = value;
            if (value)
            {
                cross.color = new Color32(255, 255, 255, 255);
                crossNumbText.color = new Color32(70, 70, 70, 255);
                crossHighlight.SetActive(true);

                circle.color = new Color32(255, 255, 255, 80);
                circleNumbText.color = new Color32(70, 70, 70, 80);
                circleHighlight.SetActive(false);
            }
            else
            {
                circle.color = new Color32(255, 255, 255, 255);
                circleNumbText.color = new Color32(70, 70, 70, 255);
                circleHighlight.SetActive(true);

                cross.color = new Color32(255, 255, 255, 80);
                crossNumbText.color = new Color32(70, 70, 70, 80);
                crossHighlight.SetActive(false);
            }
        }
    }

    private int crossCount;
    /// <summary>
    /// Количество крестиков.
    /// </summary>
    public int CrossCount
    {
        get
        {
            return crossCount;
        }
        set
        {
            crossCount = value;
            crossNumbText.text = crossCount.ToString();
        }
    }

    private int circleCount;
    /// <summary>
    /// Количество ноликов.
    /// </summary>
    public int CircleCount
    {
        get
        {
            return circleCount;
        }
        set
        {
            circleCount = value;
            circleNumbText.text = circleCount.ToString();
        }
    }
        
    public void Start ()
    {
        CrossCount = 0;
        CircleCount = 0;
        CrossTurn = true;

        player = this;
    }
}
