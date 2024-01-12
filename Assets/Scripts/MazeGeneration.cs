using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.AI.Navigation;

public class MazeGeneration : MonoBehaviour
{
    [SerializeField]
    private MazeCell mazeCellPrefab;

    [SerializeField]
    private int mazeWidth;

    [SerializeField]
    private int mazeDepth;

    private MazeCell[,] mazeGrid;

    void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }
        GenerateMaze(null, mazeGrid[0, 0]);
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        new WaitForSeconds(0.05f);
       

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);

    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        if (x+1< mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if (cellToRight.IsVisited==false)
            {
                yield return cellToRight;
            }
        }
        if (x-1>=0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.IsVisited==false)
            {
                yield return cellToLeft;
            }
        }
        if (z+1< mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];
            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }
        if (z-1>=0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if (cellToBack.IsVisited ==false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }
        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        if (previousCell.transform.localPosition.z< currentCell.transform.localPosition.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        if (previousCell.transform.localPosition.z> currentCell.transform.localPosition.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
