using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{

    [SerializeField] private MazeCell mazeCellPrefab;

    [SerializeField] private int mazeWidth;

    [SerializeField] private int mazeDepth;

    [SerializeField] private GameObject solveMazeCube;

    private MazeCell[,] mazeGrid;

    [SerializeField] private float cellSize;
    // Start is called before the first frame update
    void Start()
    {
        //transform.SetParent(mazeCellPrefab.transform);
        
        // Populate the maze

        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

          for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 115; z < mazeDepth; z++)
            {
                // Store the maze in an array
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                //mazeGrid[x, z].transform.localScale = new Vector3(0.8f, 1f, 0.8f);
            }
        }
         GenerateMaze(null, mazeGrid[0, 115]);
    }
    
    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        //Makes all the cells of the maze visible
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);
        
        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                //Repeatdly call this method moving from cell to cell until there is no unvisited cells
                GenerateMaze(currentCell, nextCell);
            }    
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }
    
    //Function to check which cells are unvisited and return the cell
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;
        

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }
        
        if (z - 1 >= 115)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        // First cell, there is no previous cell so no wall to clear
        if (previousCell == null)
            return;
        
        //Algorithm has gone from left to right, so we clear the right wall and go to the left
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        
        //Algorithm has gone from right to left, so we clear the left wall and go right
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        
        //Algorithm has gone from back to front, so we clear the front wall and go back
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        
        //Algorithm has gone from front to back, so we clear the back wall and go front
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
