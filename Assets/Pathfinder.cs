using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Pathfinder : MonoBehaviour
{
    public bool isPath;
    public Vector2Int StartPos;
    public Tile initspos;
    public Vector2Int destination;
    public GameObject  Move;
    public List<Tile> path ;
    Tile StartNode;
    Tile TergetNode;
    Tile CurrentNode;

    Queue<Tile> frontier = new Queue<Tile>();
    Dictionary<Vector2Int , Tile> reached = new Dictionary<Vector2Int, Tile>();

    Dictionary<Vector2Int , Tile> grid = new Dictionary<Vector2Int, Tile>();

    Vector2Int[] searchOrder = {
        Vector2Int.right, Vector2Int.left,
        Vector2Int.up, Vector2Int.down,
    };
    public ObstacleManager obstacleManager;

    private void Start()
    {
       // Init();
    }
    public void Init()
    {
        if(obstacleManager!= null)
        {
            grid = obstacleManager.Tilelist;
            Debug.Log(grid);
            Debug.Log(transform.name);

        }
    }
    public IEnumerator PlayerMv(int n)
    {
        for (int i = 1; i < path.Count - n; i++)
        {
            Vector3 startPosition = Move.transform.position;
            Vector3 endPosition = new Vector3( path[i].TilePos.x ,0.5f, path[i].TilePos.y);
            float travelPercent = 0f;
            Move.transform.LookAt(endPosition);
            isPath = true;
            initspos = path[i];
            if (initspos.blocked)
            {
                StopAllCoroutines();
                ObstacleManager.Instance.ResetNode();
              //  StartNode = initspos;
                path.Clear();
             //   path = GetNewPath();
               // StartCoroutine(PlayerMv(n));
                Debug.Log("bLOCKED" + StartNode);
                isPath = false;
                ObstacleManager.Instance.editor = false;
            }
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * 3f;
                Move.transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

        }
        isPath = false;
        ObstacleManager.Instance.editor = true;
        StartPos = destination;
        Move.transform.localEulerAngles = new Vector3(0, -90, 0);
        //path.Clear();
        //ObstacleManager.Instance.ResetNode();
    }
    public void SetNewDestination(Vector2Int startCoordinates, Vector2Int targetCoordinates)
    {
        StartPos = startCoordinates;
        destination = targetCoordinates;
        StartNode = ObstacleManager.Instance.Tilelist[this.StartPos] ;
        TergetNode = ObstacleManager.Instance.Tilelist[this.destination];
        path= GetNewPath();
    }
    public List<Tile> GetNewPath()
    {
        obstacleManager.ResetNode();
        BreadthFirstSearch(StartPos);
        return BuildPath();
    }
    private List<Tile> BuildPath()
    {
        List<Tile> path = new List<Tile>();
        Tile node = TergetNode;
        path.Add(node);
        node.path = true;
        while (node.ConnectedTile != null)
        {
            node = node.ConnectedTile;
            path.Add(node);
            node.path = true;
            node.DisplayNo.gameObject.SetActive(true);
        }
        path.Reverse();
        return path;
    }
    private void BreadthFirstSearch(Vector2Int StartCordinate)
    {
        StartNode.blocked = false;
        TergetNode.blocked = false;

        frontier.Clear();
        reached.Clear();

        bool Isrunning = true;

        frontier.Enqueue(ObstacleManager.Instance.Tilelist[StartCordinate]);
        reached.Add(StartCordinate, ObstacleManager.Instance.Tilelist[StartCordinate]);
        while (frontier.Count >  0 && Isrunning == true)
        {
            CurrentNode = frontier.Dequeue();
            CurrentNode.explored = true;
            ExploreNeighbors();
            if(CurrentNode.TilePos == destination)
            {
                Isrunning = false;
            }
        }
    }
    private void ExploreNeighbors()
    {
        List<Tile> neighbors = new List<Tile> ();
        foreach (Vector2Int dir in searchOrder)
        {
            Vector2Int neighborDir = CurrentNode.TilePos + dir;
            if (ObstacleManager.Instance.Tilelist.ContainsKey(neighborDir))
            {
                neighbors.Add(ObstacleManager.Instance.Tilelist[neighborDir]);
            }
        }
        foreach (Tile node in neighbors)
        {
            if (!reached.ContainsKey(node.TilePos) && node.blocked == false)
            {
                node.ConnectedTile = CurrentNode;
                frontier.Enqueue(node);
                reached.Add(node.TilePos , node);
            }
        }
    }
}