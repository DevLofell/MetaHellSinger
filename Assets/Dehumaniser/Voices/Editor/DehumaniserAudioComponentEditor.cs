using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    [CustomEditor(typeof(DehumaniserAudioComponent))]
    public class DehumaniserAudioComponentEditor : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal { }
}
