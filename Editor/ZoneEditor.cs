using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace SimpleMan.Zones.Editor
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(BaseZone), true)]
    public class ZoneEditor : OdinEditor
    {
        //------FIELDS
        private BaseZone _target;



        //------METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            _target = target as BaseZone;
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
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}