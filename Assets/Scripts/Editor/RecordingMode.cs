using UnityEngine;
using UnityEditor;
using UnityEditor.Recorder;
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

        // Start playing, pause, and maximize game view
        EditorApplication.isPaused = true;
        EditorApplication.isPlaying = true;
        EditorWindow gameView = Resources
                                    .FindObjectsOfTypeAll<EditorWindow>()
                                    .Single(window => window != null && window.GetType().FullName == "UnityEditor.GameView");
        gameView.maximized = true;
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();

        // Start recording
        RecorderWindow recorderWindow = EditorWindow.GetWindow<RecorderWindow>();
        recorderWindow.StartRecording();
    }
}
