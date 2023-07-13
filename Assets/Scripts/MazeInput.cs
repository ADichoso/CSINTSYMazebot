using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MazeInput : MonoBehaviour
{
    public TMP_InputField gridInputField;

    public List<MazeButton> mazeButtons = new List<MazeButton>();
    public GameObject buttonPrefab;
    public GameObject rowGroupPrefab;

    public VerticalLayoutGroup vertLayoutGroup;
    public GameObject buttonsPanel;
    public float size = 800f;
    int gridDim;

    List<GameObject> rowObjects= new List<GameObject>();
    public void initializeGridButtons() 
    {
        deleteButtons();
        try
        {
            gridDim = int.Parse(gridInputField.text);

            float buttonSize = size / Mathf.Min(gridDim, gridDim) * 0.9f;
            for (int i = 0; i < gridDim; i++)
            {
                GameObject rowGroup = Instantiate(rowGroupPrefab, buttonsPanel.transform);
                rowObjects.Add(rowGroup);
                for (int j = 0; j < gridDim; j++)
                {
                    GameObject newButton = Instantiate(buttonPrefab, rowGroup.transform);

                    newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonSize, buttonSize);
                    mazeButtons.Add(newButton.GetComponent<MazeButton>());
                }
            }

            mazeButtons[0].setInitialButton();
            mazeButtons[mazeButtons.Count - 1].setGoalButton();
        }
        catch 
        {
            Debug.Log("PLEASE ENTER A NUMBER");
        }
    }

    public void deleteButtons() 
    {
        foreach (MazeButton mazeButton in mazeButtons)
            Destroy(mazeButton.gameObject);

        foreach (GameObject row in rowObjects)
            Destroy(row);

        rowObjects.Clear();
        mazeButtons.Clear();
    }
    public void translateButtonsToTiles() 
    {
        GridManager.Instance.GridX = gridDim;
        GridManager.Instance.GridY = gridDim;

        if (GridManager.Instance != null) GridManager.Instance.initializeGrid();

        for (int i = 0; i < mazeButtons.Count; i++)
            if (mazeButtons[i].isWall)
                GridManager.Instance.TileSet[i].setAsWall();
        
        if (MovementManager.Instance != null) MovementManager.Instance.initializeSearch();

        gameObject.SetActive(false);
    }
}