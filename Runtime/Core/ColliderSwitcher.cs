using Sirenix.OdinInspector;
using UnityEngine;


namespace SimpleMan.Zones.Editor
{
    [AddComponentMenu(Editor.EditorConstants.MENU_PATH + "Collider Switcher")]
    public sealed class ColliderSwitcher : MonoBehaviour
    {
        public enum ColliderType
        {
            None,
            Box,
            Sphere
        }




        //------PROPERTIES
        [ShowInInspector]
        [EnumToggleButtons]
        public ColliderType CurrentCollider
        {
            get
            {
                Collider collider = GetComponent<Collider>();
                if (collider == null)
                    return ColliderType.None;

                if(collider is SphereCollider)
                    return ColliderType.Sphere;

                else if(collider is BoxCollider)
                    return ColliderType.Box;

                else
                    return ColliderType.None;
            }
            set
            {
                Collider collider = GetComponent<Collider>();
                if (collider != null)
                {
                    DestroyImmediate(collider, true);
                }

                switch (value)
                {
                    case ColliderType.Sphere:
                        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                        sphereCollider.isTrigger = true; break;

                    case ColliderType.Box:
                        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                        boxCollider.isTrigger = true; break;
                }
            }
        }
    }
}