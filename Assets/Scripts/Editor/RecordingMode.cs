using UnityEngine;
using UnityEditor;
using System.Linq;

public class RecordingMode
{
    [MenuItem("Tools/GetTheMemo/Enter Recording Mode")]
    private static void EnterRecordingMode()
    {
        // Create recording track
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RecordingTrack.prefab", typeof(GameObject));
        GameObject.Instantiate(prefab);

        // Run all custom recorder scripts
        foreach (var recorder in Object.FindObjectsOfType<MonoBehaviour>(true).OfType<IRecordMode>())
        {
            recorder.OnEnterRecordMode();
        }
    }
}
