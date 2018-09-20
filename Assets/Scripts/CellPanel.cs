using UnityEngine;

public class CellPanel : MonoBehaviour
{
    // Клетка с поля, на которой нужно сделать ход.
    [HideInInspector]
    public GameObject sender;

    // Индекс стенки (от 0 до 3, -1 - для знака).
    public int Index;
    
    // Нажатие на клетку панели.
    private void OnMouseDown()
    {
        // Запускаем функцию нажатия клетки на панели из игрового менеджера и передаем в нее данную клетку.
        GameManager.instance.PanelCellPressed(gameObject);
    }
}
