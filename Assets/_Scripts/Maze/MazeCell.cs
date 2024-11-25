using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject leftWall, rightWall, frontWall, backWall, unvisitedBlock;
    
    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
        leftWall.transform.localScale = new Vector3(10f, 1f, 2.2f);
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);
        rightWall.transform.localScale = new Vector3(10f, 1f, 2.2f);
    }
    
    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
        frontWall.transform.localScale = new Vector3(10f, 1f, 2.2f);
    }

    public void ClearBackWall()
    {
        backWall.SetActive(false);
        backWall.transform.localScale = new Vector3(10f, 1f, 2.2f);
    }
}
