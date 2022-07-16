using System.Collections.Generic;
using UnityEngine;


namespace SimpleMan.Zones.Editor
{
    public class ZoneTypesCollection : ScriptableObject
	{
		//------FIELDS
		[SerializeField]
		private GameObject[] _prefabs;





        //------PROPERTIES
        public IReadOnlyList<GameObject> Prefabs
        {
            get => _prefabs;
        }
    }
}