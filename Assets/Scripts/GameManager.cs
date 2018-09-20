using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Ссылка на действующую версию компонента (она сейчас заведует игрой).
    public static GameManager instance;

    // Массив всевозможных спрайтов для клеток на игровом поле.
    public Sprite[] sprites;

    // Фон выбранной клетки на игровом поле.
    public GameObject cellBackground;

    // Панель с возможными ходами.
    public GameObject possibleMoves;

    // Префабы для клеток с возможными ходами.
    public GameObject[] cell_p;

    // Результат игры.
    public Transform result;

    // Экран игры.
    public GameObject playScreen;

    // Экран конца игры.
    public GameObject end;
        
    // Потомки от панели (сделано для того, чтобы можно было их очищать).
    private List<GameObject> panelChildren;

    // Индекс неактивной стены (для случаев, когда в клетку можно поставить знак).
    private int inactiveWall;

    // Количество клеток со знаками.
    private int signedCells;
        
    /// <summary>
    /// Удаляет все ходы с панели возможных ходов.
    /// </summary>
    private void ClearChildren()
    {
        // Удаляем каждого ребеночка.
        panelChildren.ForEach(Destroy);

        panelChildren = new List<GameObject>();
    }

    /// <summary>
    /// Находит клетку по ее координатам.
    /// </summary>
    /// <param name="x">координата x клетки</param>
    /// <param name="y">координата y клетки</param>
    /// <returns>найденная клетка</returns>
    private GameObject FindCell(int x, int y)
    {
        // Найдем родительский трансформ поля.
        Transform field = GetComponent<FieldGenerator>().cellsHandler;

        foreach(Transform child in field)
        {
            CellField cf = child.gameObject.GetComponent<CellField>();

            // Если клетка имеет заданные координаты.
            if (cf != null && cf.x == x && cf.y == y)
                return child.gameObject;
        }

        return null;
    }

    /// <summary>
    /// Запускается, когда нажата клетка на игровом поле.
    /// </summary>
    /// <param name="cell">нажатая клетка</param>
    public void FieldCellPressed(GameObject cell)
    {
        GetComponent<PlayAudio>().FieldAudio();

        inactiveWall = -1;

        // Находим в переданной клетке ее игровую компоненту.
        CellField cf = cell.GetComponent<CellField>();

        // Выделяем нажатую клетку поля цветом.
        cellBackground.transform.position = cf.transform.position;
        cellBackground.SetActive(true);

        // Удаляем все предыдущие ходы с панели.
        ClearChildren();

        // Если панель не активна и есть возможные ходы, то сделать ее активной.
        if (cf.signed)
        {
            possibleMoves.SetActive(false);
        }
        else if (!possibleMoves.activeSelf)
        {
            possibleMoves.SetActive(true);
        }

        if (possibleMoves.activeSelf)
        {
            // Проверяем для каждой стенки, стоит ли она на на данной клетке или нет.
            // Если нет - добавляем соответствующий возможный ход на панель. 
            for (int i = 0; i < cf.Walls.Length; i++)
            {
                if (!cf.Walls[i])
                {
                    GameObject possibleMove = Instantiate(cell_p[i], possibleMoves.transform);
                    possibleMove.GetComponent<CellPanel>().sender = cell;
                    inactiveWall = i;

                    // Добавляем в массив потомков нашей панели еще один возможный ход.
                    panelChildren.Add(possibleMove);
                }
            }

            // Возможность поставить крестик/нолик.
            if ((panelChildren.Count == 1 || panelChildren.Count == 0) && !cf.signed)
            {
                GameObject possibleMove = Instantiate(cell_p[4 + (Player.player.CrossTurn ? 0 : 1)], possibleMoves.transform);
                possibleMove.GetComponent<CellPanel>().sender = cell;
                ClearChildren();
                panelChildren.Add(possibleMove);
            }
        }
    }

    /// <summary>
    /// Запускается, когда выбран ход на панели.
    /// </summary>
    /// <param name="cell">выбранный ход</param>
    public void PanelCellPressed(GameObject cell)
    {
        // Если панель активна, то сделать ее неактивной.
        if (possibleMoves.activeSelf)
        {
            possibleMoves.SetActive(false);
        }

        // Находим в переданной клетке ее игровую компоненту.
        CellPanel cp = cell.GetComponent<CellPanel>();

        // Для клетки, на которую мы нажали и захотели сделать ход на ней, добавляем новую стенку.
        CellField sender = cp.sender.GetComponent<CellField>();

        // Индекс стенки, которую нужно активировать.
        int ind = panelChildren.Count == 1 ? inactiveWall : cp.Index;

        sender.AddSprite(ind, panelChildren.Count == 1);

        if (ind >= 0)
        {
            // Куда нужно сдвинуться от данной клеточки, чтобы поставить соседнюю стенку.
            Vector2 shift = new Vector2((ind - 1) % 2, -(ind - 2) % 2);

            // Находим соседа данной клеточки.
            GameObject neighbour = FindCell(sender.x + (int)shift.x, sender.y + (int)shift.y);

            // Меняем спрайт соседа, если он есть.
            if (neighbour != null)
            {
                neighbour.GetComponent<CellField>().AddSprite((ind + 2) % 4, false);
            }
        }    

        // Если поставили знак, увеличиваем количество клеток со знаками и проверяем, закончилась ли игра.
        if (sender.signed)
        {
            signedCells++;

            if (signedCells == GetComponent<FieldGenerator>().cellsHandler.childCount)
            {
                WinOrLose();
            }
        }

        GetComponent<PlayAudio>().PanelAudio();

        cellBackground.SetActive(false);

        // Меняем очередь.
        Player.player.CrossTurn = !Player.player.CrossTurn;
    }

    /// <summary>
    /// Определяет, кто выиграл.
    /// </summary>
    private void WinOrLose()
    {
        playScreen.SetActive(false);
        end.SetActive(true);

        ShowResult();
    }

    /// <summary>
    /// Показывает результат игры.
    /// </summary>
    private void ShowResult()
    {
        // Итоговое количество крестиков и ноликов.
        int cross = Player.player.CrossCount,
            circle = Player.player.CircleCount;

        if (cross > circle)
        {
            result.FindChild("X Won").gameObject.SetActive(true);
        }
        else if(cross < circle)
        {
            result.FindChild("O Won").gameObject.SetActive(true);
        }
        else
        {
            result.FindChild("Draw").gameObject.SetActive(true);
        }

        GetComponent<PlayAudio>().EndAudio();
    }

    /// <summary>
    /// Делает неактивными варианты результата игры (детей result).
    /// </summary>
    private void DeactivateResults()
    {
        foreach (Transform child in result)
        {
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Начинает игру заново (все объеты возвращаются в исходное состояние).
    /// </summary>
    public void Restart()
    {
        //Удаляем все клетки игрового поля.
        foreach (Transform child in GetComponent<FieldGenerator>().cellsHandler)
        {
            Destroy(child.gameObject);
        }

        ClearChildren();

        GetComponent<Player>().Start();
        GetComponent<FieldGenerator>().GenerateField();
        Start();

        GetComponent<PlayAudio>().StartAudio();
    }
    
    private void Start()
    {
        DeactivateResults();
        possibleMoves.SetActive(false);

        panelChildren = new List<GameObject>();
        
        signedCells = 0;

        instance = this;

        cellBackground.SetActive(false);
    }
}
