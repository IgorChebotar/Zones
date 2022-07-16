using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SimpleMan.Zones.Editor.EditorConstants;

namespace SimpleMan.Zones.Editor
{
    internal static class HierarchyObjectCreator
    {
        //------METHODS
        [MenuItem("GameObject/Zones/Event Zone", false)]
        public static void CreateEventZone(MenuCommand menuCommand)
        {
            int index = 0;
            var prefabs = GetPrefabs();
            if(index >= prefabs.Count)
            {
                throw new System.NullReferenceException(
                    $"<b> Zones: </b> Can't find prefab at index '{index}' in collection. " +
                    "Probably prefab was delated. Go to Edit -> Project settings -> Zones and check " +
                    "the prefabs list, than press 'Apply' button.");
            }

            GameObject prefab = prefabs[index];
            if(prefab == null)
            {
                throw new System.NullReferenceException(
                    "<b> Zones: </b> Zones collection have null reference " +
                    "to prefab.Check collection asset");
            }

            GameObject gameObject = Object.Instantiate(prefab);
            gameObject.name = prefab.name;
            GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);

            GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(gameObject, "Create" + gameObject.name);
            Selection.activeObject = gameObject;
        }

        private static IReadOnlyList<GameObject> GetPrefabs()
        {
            var collectionAsset = AssetDatabase.LoadAssetAtPath<ZoneTypesCollection>(COLLECTION_PATH);
            if(collectionAsset == null)
            {
                throw new System.NullReferenceException(
                    "<b> Zones: </b> Zones collection asset was not found");
            }

            return collectionAsset.Prefabs;
        }
    }

}
