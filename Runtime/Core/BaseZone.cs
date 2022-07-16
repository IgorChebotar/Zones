using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


namespace SimpleMan.Zones
{
    public abstract class BaseZone : MonoBehaviour
    {
        //------FIELDS
        [BoxGroup("Base")]
        [SerializeField]
        private bool _printLogs = true;




        //------PROPERTIES
        public bool PrintLogs
        {
            get => _printLogs;
            set => _printLogs = value;
        }




        //------EVENTS
        [FoldoutGroup("Events", Order = 90)]
        [SerializeField]
        private protected UnityEvent<GameObject>
            _onObjectEntered = new UnityEvent<GameObject>(),
            _onObjectCameOut = new UnityEvent<GameObject>();




        //------METHODS
        private void OnTriggerEnter(Collider other)
        {
            ProcessEntering(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ProcessComingOut(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ProcessEntering(collision.collider);
        }

        private void OnCollisionExit(Collision collision)
        {
            ProcessComingOut(collision.collider);
        }

        private protected abstract void ProcessEntering(Collider other);

        private protected abstract void ProcessComingOut(Collider other);
    }
}