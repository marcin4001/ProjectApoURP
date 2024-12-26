using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof (EnemyController))]
public class EnemyControllerEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EnemyController enemy = (EnemyController)target;
        if(GUILayout.Button("Randomize Id"))
        {
            enemy.RandomizeID();
            EditorUtility.SetDirty(target);
        }
    }

}
