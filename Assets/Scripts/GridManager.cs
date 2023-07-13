using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private void Awake()
    {
        Instance = this;
        if (this != Instance) Debug.LogWarning("WARNING! MORE THAN 1 INSTANCE OF GridManager DETECTED!");
    }

    //Get Prefab of Platform

    [SerializeField]
    private int gridX = 8;

    [SerializeField]
    private int gridY = 8;

    [SerializeField]
    private float marginX = 2.0f;

    [SerializeField]
    private float marginY = 2.0f;
    
    public GameObject platformPrefab;

    public List<GridTile> TileSet = new List<GridTile>();
    public GridTile initialTile= null;
    public GridTile goalTile = null;

    public Material unexploredMat;
    public Material sensedMat;
    public Material exploredMat;
    public Material traveledMat;
    public Material initialMat;
    public Material goalMat;

    public int GridX { get => gridX; set => gridX = value; }
    public int GridY { get => gridY; set => gridY = value; }

    //Initialize the grid, make an x by my grid of platformprefab objects, with 
    public void initializeGrid()
    {

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //Insantiate a platform object
                GameObject new_plat = createGridTile(x, y);
                new_plat.transform.Translate(
                    new Vector3(
                    (new_plat.transform.lossyScale.z + marginY) * y,
                    0,
                    (new_plat.transform.lossyScale.x + marginX) * x
                    )
                    );

                TileSet.Add(new_plat.GetComponent<GridTile>());
            }
        }

        initialTile = TileSet[0];
        goalTile = TileSet[TileSet.Count - 1];

        foreach (GridTile tile in TileSet)
        {
            setAdjacencyList(tile);
            tile.tileInfo.heuristicCost = getManhattanDistance(tile);
        }

        initialTile.GetComponent<MeshRenderer>().material = initialMat;
        goalTile.GetComponent<MeshRenderer>().material = goalMat;

        Debug.Log("Grid Initialized");
    }

    public GameObject createGridTile(int x, int y) 
    {
        GameObject tile = Instantiate(platformPrefab, transform.position, Quaternion.identity, transform);

        tile.GetComponent<GridTile>().tileInfo.x_coord = x;
        tile.GetComponent<GridTile>().tileInfo.y_coord = y;

        return tile;
    }

    public void setAdjacencyList(GridTile tile) 
    {
        foreach (GridTile other_tile in TileSet)
            if (areTilesAdjacent(tile, other_tile)) tile.adjacentTiles.Add(other_tile);

        if (tile.adjacentTiles.Count > 4 || tile.adjacentTiles.Count < 1) Debug.LogWarning("WARNING! ADJACENCY CHECK FAILED!");
    }

    bool areTilesAdjacent(GridTile tile1, GridTile tile2) 
    {
        int x1 = tile1.tileInfo.x_coord;
        int y1 = tile1.tileInfo.y_coord;
        int x2 = tile2.tileInfo.x_coord;
        int y2 = tile2.tileInfo.y_coord;

        return
            (x1 == x2 && (y1 == y2 + 1 || y2 == y1 + 1)) ||
            (y1 == y2 && (x1 == x2 + 1 || x2 == x1 + 1));
    }

    public GridTile getTileFromCoordinate(int x, int y) 
    {
        if (x >= 0 && x < gridX && y >= 0 || y < gridY)
            //Tile def exists, just find it in the array
            foreach (GridTile tile in TileSet)
                if (tile.tileInfo.x_coord == x && tile.tileInfo.y_coord == y)
                    return tile;

        return null;
    }

    public int getManhattanDistance(GridTile tile)
    {
        int x1 = tile.tileInfo.x_coord;
        int y1 = tile.tileInfo.y_coord;
        int x2 = goalTile.tileInfo.x_coord;
        int y2 = goalTile.tileInfo.y_coord;

        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }

    public GridTile findTile(TileInfo tileInfo)
    {
        foreach (GridTile tile in TileSet)
            if (tileInfo.x_coord == tile.tileInfo.x_coord && tileInfo.y_coord == tile.tileInfo.y_coord) return tile;

        return null;
    }
}
