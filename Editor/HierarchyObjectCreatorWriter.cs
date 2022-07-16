using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SimpleMan.Utilities;
using static SimpleMan.Zones.Editor.EditorConstants;

namespace SimpleMan.Zones.Editor
{
    internal static class HierarchyCreatorWriter
    {
        public static void UpdateCreatorClass()
        {
            using(StreamWriter outfile = new StreamWriter(HIERARCHY_CREATOR_PATH))
            {
                void AddSection(string name, int prefabIndex)
                {
                    outfile.WriteLine($"        [MenuItem(\"GameObject/Zones/{name.ToSplitPascalCase()}\", false)]");
                    outfile.WriteLine($"        public static void Create{name.WithoutSpaces()}(MenuCommand menuCommand)");
                    outfile.WriteLine("        {");
                    outfile.WriteLine($"            int index = {prefabIndex};");
                    outfile.WriteLine("            var prefabs = GetPrefabs();");
                    outfile.WriteLine("            if(index >= prefabs.Count)");
                    outfile.WriteLine("            {");
                    outfile.WriteLine("                throw new System.NullReferenceException(");
                    outfile.WriteLine("                    $\"<b> Zones: </b> Can't find prefab at index '{index}' in collection. \" +");
                    outfile.WriteLine("                    \"Probably prefab was delated. Go to Edit -> Project settings -> Zones and check \" +");
                    outfile.WriteLine("                    \"the prefabs list, than press 'Apply' button.\");");
                    outfile.WriteLine("            }");
                    outfile.WriteLine("");
                    outfile.WriteLine("            GameObject prefab = prefabs[index];");
                    outfile.WriteLine("            if(prefab == null)");
                    outfile.WriteLine("            {");
                    outfile.WriteLine("                throw new System.NullReferenceException(");
                    outfile.WriteLine("                    \"<b> Zones: </b> Zones collection have null reference \" +");
                    outfile.WriteLine("                    \"to prefab.Check collection asset\");");
                    outfile.WriteLine("            }");
                    outfile.WriteLine("");
                    outfile.WriteLine("            GameObject gameObject = Object.Instantiate(prefab);");
                    outfile.WriteLine("            gameObject.name = prefab.name;");
                    outfile.WriteLine("            GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);");
                    outfile.WriteLine("");
                    outfile.WriteLine("            GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);");
                    outfile.WriteLine("            Undo.RegisterCreatedObjectUndo(gameObject, \"Create\" + gameObject.name);");
                    outfile.WriteLine("            Selection.activeObject = gameObject;");
                    outfile.WriteLine("        }");
                    outfile.WriteLine("");
                }

                outfile.WriteLine("using System.Collections.Generic;");
                outfile.WriteLine("using UnityEditor;");
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using static SimpleMan.Zones.Editor.EditorConstants;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace SimpleMan.Zones.Editor");
                outfile.WriteLine("{");
                outfile.WriteLine("    internal static class HierarchyObjectCreator");
                outfile.WriteLine("    {");
                outfile.WriteLine("        //------METHODS");

                var prefabs = GetPrefabs();
                for (int i = 0; i < prefabs.Count; i++)
                {
                    if (prefabs == null)
                    {
                        throw new System.NullReferenceException(
                            "<b> Zones: </b> Zones collection have null reference element. " +
                            "Check collection in Edit -> Project settings -> Zones");
                    }

                    if (prefabs[i] == null)
                    {
                        throw new System.NullReferenceException(
                            "<b> Zones: </b> Zones collection have null reference element. " +
                            "Check collection in Edit -> Project settings -> Zones");
                    }
                    AddSection(prefabs[i].name, i);
                }


                outfile.WriteLine("        private static IReadOnlyList<GameObject> GetPrefabs()");
                outfile.WriteLine("        {");
                outfile.WriteLine("            var collectionAsset = AssetDatabase.LoadAssetAtPath<ZoneTypesCollection>(COLLECTION_PATH);");
                outfile.WriteLine("            if(collectionAsset == null)");
                outfile.WriteLine("            {");
                outfile.WriteLine("                throw new System.NullReferenceException(");
                outfile.WriteLine("                    \"<b> Zones: </b> Zones collection asset was not found\");");
                outfile.WriteLine("            }");
                outfile.WriteLine("");
                outfile.WriteLine("            return collectionAsset.Prefabs;");
                outfile.WriteLine("        }");
                outfile.WriteLine("    }");
                outfile.WriteLine("");
                
                outfile.WriteLine("}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static IReadOnlyList<GameObject> GetPrefabs()
        {
            var collectionAsset = AssetDatabase.LoadAssetAtPath<ZoneTypesCollection>(COLLECTION_PATH);
            if (collectionAsset == null)
            {
                throw new System.NullReferenceException(
                    "<b> Zones: </b> Zones collection asset was not found");
            }

            return collectionAsset.Prefabs;
        }
    }
}