using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;


namespace SimpleMan.Zones.Editor
{
    [CustomEditor(typeof(Visualizer))]
    [CanEditMultipleObjects]
    public class VisualizerEditor : OdinEditor
    {
        //------FIELDS
        private Visualizer _target;



        //------PROPERTIES




        //------EVENTS




        //------METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            _target = (Visualizer)target;
        }

        public override void OnInspectorGUI()
        {
            if (!_target.TryGetComponent(out Collider collider))
            {
                EditorGUILayout.HelpBox(
                    "No collider. Add collider to this game object", MessageType.Error);

                if (GUILayout.Button("Fix it!"))
                {
                    SphereCollider sphereCollider = _target.gameObject.AddComponent<SphereCollider>();
                    sphereCollider.isTrigger = true;
                }
            }

            else if(
                collider is SphereCollider is false &&
                collider is BoxCollider is false)
            {
                EditorGUILayout.HelpBox(
                    "Only sphere and box colliders supported", MessageType.Error);

                if (GUILayout.Button("Fix it!"))
                {
                    Collider[] colliders = _target.GetComponents<Collider>();
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        DestroyImmediate(colliders[i]);
                    }

                    _target.gameObject.AddComponent<SphereCollider>();
                }
            }

            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}