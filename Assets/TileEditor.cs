using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TileSO))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        for (int y = 0; y < 10; y++)
        {
            EditorGUILayout.BeginVertical();
            for (int x = 0; x < 10; x++)
            {
                if(ObstacleManager.Instance != null)
                {
                    if (!ObstacleManager.Instance.PlayerPathfinder.isPath)
                    {
                        ObstacleManager.Instance.tileSO.Tile[x, y] = EditorGUILayout.Toggle(ObstacleManager.Instance.tileSO.Tile[x, y]);
                        if (ObstacleManager.Instance.tileSO.Tile[x, y] == true)
                        {
                            ObstacleManager.Instance.Tilelist[new Vector2Int(x, y)].RenderObj.material = ObstacleManager.Instance.BlockedMat;
                            ObstacleManager.Instance.Tilelist[new Vector2Int(x, y)].blocked = true;
                        }
                        else
                        {
                            ObstacleManager.Instance.Tilelist[new Vector2Int(x, y)].RenderObj.material = ObstacleManager.Instance.Valid;
                            ObstacleManager.Instance.Tilelist[new Vector2Int(x, y)].blocked = false;
                        }
                    }


                }
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();

    }
}
