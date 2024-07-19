using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    class RageMonsterAbout : EditorWindow
    {
        [MenuItem("Dehumaniser/RageMonster/About")]
        static void Init()
        {
            var window = ScriptableObject.CreateInstance<RageMonsterAbout>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 280, 40);
            window.ShowAuxWindow();
        }

        void OnGUI()
        {
            string version = "v1.0";

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Dehumaniser RageMonster, Version: " + version);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class RageMonster : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal
    {
        [MenuItem("Dehumaniser/RageMonster/AudioComponent")]
        static void Create()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null)
            {
                return;
            }

            GameObject component = new GameObject("RageMonster");

            component.transform.parent = target.transform;

            component.AddComponent<DehumaniserAudioComponent>();

            component.AddComponent<DehumaniserRageMonster>();
        }
    }

    [CustomEditor(typeof(DehumaniserRageMonster))]
    public class RageMonsterEditor : Fabric.Dehumaniser.DehumaniserDSPVoiceEditor { }
}
