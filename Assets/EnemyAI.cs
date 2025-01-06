using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public Pathfinder pathfinder;
    private void Update()
    {
        if (!pathfinder.isPath)
        {
            pathfinder.isPath = false;
            pathfinder.SetNewDestination(pathfinder.StartPos,
             ObstacleManager.Instance.PlayerPathfinder.StartPos
              );
            StartCoroutine(pathfinder.PlayerMv(1));
        }
    }
}
