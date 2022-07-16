using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using SimpleMan.Utilities;


namespace SimpleMan.Zones
{
    /// <summary>
    /// Base class of all trigger zones. Stores (registers) items 
    /// inside itself
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Zone<T> : BaseZone where T : Component
    {
        //------FIELDS
        [BoxGroup("Base")]
        [SerializeField]
        private bool _useTags = false;

        [BoxGroup("Base")]
        [ShowIf(nameof(_useTags))]
        [SerializeField]
        private string[] _allowedTags = new string[] { };
        private List<T> _registeredObjects = new List<T>(1024);




        //------PROPERTIES
        public bool UseTags
        {
            get => _useTags;
            set => _useTags = value;
        }
        public IReadOnlyList<string> AllowedTags => _allowedTags;
        public IReadOnlyList<T> RegisteredObjects => _registeredObjects;




        //------EVENTS
        public event Action<BaseZone, T> OnObjectEntered;
        public event Action<BaseZone, T> OnObjectCameOut;




        //------METHODS
        /// <summary>
        /// Called after object entered in zone. For valid objects only.
        /// Can be fully overwrited (without 'base.*' line)
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void ValidObjectEnteredHandler(Collider collider, T component)
        {

        }

        /// <summary>
        /// Called after object came out from zone. For valid objects only.
        /// Can be fully overwrited (without 'base.*' line)
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void ValidObjectCameOutHandler(Collider collider, T component)
        {

        }

        /// <summary>
        /// Use this method for create your custom registration condition. Can be 
        /// fully overrided (without 'base.*' line)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool CanBeRegistered(T other)
        {
            return CanContinueWithTag(other);
        }

        private bool CanContinueWithTag(T other)
        {
            if (!_useTags)
                return true;

            else
                return _allowedTags.Contains(other.tag);
        }

        private protected override void ProcessEntering(Collider other)
        {
            T component = other.GetComponent<T>();
            if (component == null)
                return;

            if (!CanContinueWithTag(component))
                return;

            if (!CanBeRegistered(component))
                return;

            _registeredObjects.Add(component);
            ValidObjectEnteredHandler(other, component);

            if (PrintLogs)
            {
                this.PrintLog(
                    $"Object '{other.GetNameWithoutPrefix()}' entered");
            }

            _onObjectEntered.Invoke(other.gameObject);
            OnObjectEntered?.Invoke(this, component);
        }

        private protected override void ProcessComingOut(Collider other)
        {
            T component = other.GetComponent<T>();
            if (component == null)
                return;

            if (!_registeredObjects.Contains(component))
                return;

            ValidObjectCameOutHandler(other, component);
            _registeredObjects.Remove(component);

            if (PrintLogs)
            {
                this.PrintLog(
                    $"Object '{other.GetNameWithoutPrefix()}' came out");
            }

            _onObjectCameOut.Invoke(other.gameObject);
            OnObjectCameOut?.Invoke(this, component);
        }
    }
}