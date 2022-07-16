using SimpleMan.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SimpleMan.Zones.Editor
{
    public sealed class ZoneTypesCollection : ScriptableObject
	{
		//------FIELDS
		[SerializeField]
		private GameObject[] _prefabs;





        //------PROPERTIES
        public IReadOnlyList<GameObject> Prefabs
        {
            get => _prefabs;
        }




        //------METHODS
        private void OnValidate()
        {
            _prefabs = _prefabs.Validate().ToArray();
        }
    }
}