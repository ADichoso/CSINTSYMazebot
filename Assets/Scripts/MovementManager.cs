using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;
    private void Awake()
    {
        Instance = this;
        if (this != Instance) Debug.LogWarning("WARNING! MORE THAN 1 INSTANCE OF MovementManager DETECTED!");
    }

    [SerializeField]
    private PlayerScript player;

    [SerializeField]
    private GridTile selectedGridTile;

    GridTile goalTile;
    GridTile initialTile;
    GridTile currentNode;


    bool foundGoal;
    Frontier frontier;
    List<TileInfo> reached;

    public Material frontierMat;
    public float simulationTime = 0.25f;
    public void initializeSearch() 
    {
        foundGoal = false;
        frontier = new Frontier();
        reached = new List<TileInfo>();

        goalTile = GridManager.Instance.goalTile;
        initialTile = GridManager.Instance.initialTile;

        currentNode = initialTile;
        currentNode.isSensed = true;
        currentNode.displayAdjacentUnexploredTiles();
        reached.Add(new TileInfo(currentNode.tileInfo));
        frontier.queue.Add(currentNode);

        movePlayer(currentNode);
    }
    public void movePlayer(GridTile newTile) 
    {
        if (newTile != null)
        {
            selectedGridTile = newTile;
            player.movePlayerToTarget(selectedGridTile.move_location.transform.position);
            
            if(newTile != GridManager.Instance.initialTile && newTile != GridManager.Instance.goalTile) 
                newTile.GetComponent<MeshRenderer>().material = GridManager.Instance.traveledMat;
        }   
    }

    public void movePlayerToAdjacentTile(int xOffset, int yOffset) 
    {
        if(selectedGridTile != null)
            movePlayer(
                GridManager.Instance.getTileFromCoordinate(
                    selectedGridTile.tileInfo.x_coord + xOffset, selectedGridTile.tileInfo.y_coord + yOffset
                    )
                );
    }

    public void movePlayerToAdjacentTile(GridTile tile) 
    {
        if (selectedGridTile != null)
            movePlayer(tile);
    }
    public void moveUp() {movePlayerToAdjacentTile(0, 1);}
    public void moveDown() { movePlayerToAdjacentTile(0,-1);}
    public void moveLeft() { movePlayerToAdjacentTile(1, 0); }
    public void moveRight() { movePlayerToAdjacentTile(-1, 0); }
    public IEnumerator nextSearchLevel() 
    {
        while (!foundGoal && frontier.queue.Count > 0) 
        {
            currentNode = frontier.pop();
            if (currentNode != initialTile && currentNode != goalTile) currentNode.GetComponent<MeshRenderer>().material = GridManager.Instance.exploredMat;

            Debug.Log("Searching in tile with cost: " + (currentNode.tileInfo.pathCost + currentNode.tileInfo.heuristicCost) + "=" + currentNode.tileInfo.pathCost + "+" + currentNode.tileInfo.heuristicCost);
            foreach (GridTile child in currentNode.expandTiles())
            {
                if (child == goalTile)
                {
                    child.exploreTile(currentNode);

                    foundGoal = true;
                    break;
                }

                TileInfo searchChildInfoInReached = reached.Find(tileInfo => tileInfo.x_coord == child.tileInfo.x_coord && tileInfo.y_coord == child.tileInfo.y_coord);

                if (searchChildInfoInReached == null || child.tileInfo.pathCost < searchChildInfoInReached.pathCost) 
                {
                    child.exploreTile(currentNode);

                    if (searchChildInfoInReached == null)
                        reached.Remove(searchChildInfoInReached);
                    
                    reached.Add(new TileInfo(child.tileInfo));
                    frontier.queue.Add(child);
                }
            }

            if (foundGoal) break;

            frontier.sort();
            frontier.queue[0].GetComponent<MeshRenderer>().material = frontierMat;
            
            foreach (TileInfo tileInfo in reached) 
            {
                GridTile tile = GridManager.Instance.findTile(tileInfo);
                if(tile != null) tile.displayAdjacentUnexploredTiles();
            }
                

            yield return new WaitForSeconds(simulationTime);
        }

        if (foundGoal)
        {
            currentNode = frontier.pop();
            if (currentNode != null && currentNode != initialTile && currentNode != goalTile) 
                currentNode.GetComponent<MeshRenderer>().material = GridManager.Instance.exploredMat;

            StartCoroutine(goToGoal());
        }
        else
            Debug.Log("GOAL IS UNREACHABLE");
    }
    public void approachGoal() 
    {
        StartCoroutine(nextSearchLevel());
    }

    IEnumerator goToGoal() 
    {
        List<GridTile> solution = getSolutionSpace();
        int movedTiles = 0;
        //Go through solution space, and make the user move to each space after some delay
        while(solution.Count > 0) 
        {
            GridTile tile = solution[0];
            solution.RemoveAt(0);

            movePlayerToAdjacentTile(tile);
            movedTiles++;
            Debug.Log("Going to: (" + tile.tileInfo.x_coord + "," + tile.tileInfo.y_coord + ")");
            yield return new WaitForSeconds(simulationTime);
        }

        Debug.Log("REACHED GOAL, took " + movedTiles + " moves!");
    }

    public List<GridTile> getSolutionSpace() 
    {
        List<GridTile> solution = new List<GridTile>();

        GridTile tile = GridManager.Instance.goalTile;

        while (tile != null) 
        {
            solution.Add(tile);
            tile = tile.parent;
        }
        solution.Reverse();
        solution.RemoveAt(0);
        return solution;
    }
    
}

/*
 public void bestFirstSearch() 
    {

        GridTile goalTile = GridManager.Instance.goalTile;
        GridTile currentNode = GridManager.Instance.initialTile;
        currentNode.isSensed = true;

        bool foundGoal = false;
        Frontier frontier = new Frontier();
        List<GridTile> reached = new List<GridTile>();

        frontier.queue.Add(currentNode);
        do
        {
            currentNode = frontier.pop();

            foreach (GridTile child in currentNode.expandTiles())
            {
                if (child == goalTile) 
                {
                    child.isSensed = true;
                    child.parent = currentNode;

                    foundGoal = true;
                    break;
                }

                GridTile searchChildInReached = reached.Find(x => x.Equals(child));
                if (searchChildInReached == null || child.pathCost < searchChildInReached.pathCost)
                {
                    child.isSensed = true;
                    child.parent = currentNode;

                    reached.Add(child);
                    frontier.queue.Add(child);
                }
            }
            frontier.sort();
        } while (frontier.queue.Count > 0);

        if (foundGoal)
            Debug.Log("FOUND THE GOAL");
        else
            Debug.Log("GOAL IS UNREACHABLE");
    }
 */