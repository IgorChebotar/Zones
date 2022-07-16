using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using static SimpleMan.Zones.Editor.EditorConstants;

namespace SimpleMan.Zones.Editor
{
    internal static class ProjectSettingsTab
    {
        //------METHODS
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/Zones", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "Zones",

                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                activateHandler = (searchContext, rootElement) =>
                {
                    var collection = GetCollectionAsset();
                    var collectionAsset = new SerializedObject(collection);

                    VerticalLayout verticalLayout = new VerticalLayout();
                    rootElement.Add(verticalLayout);

                    HeaderLabel label = new HeaderLabel("Zones");
                    verticalLayout.Add(label);

                    void DrawDescriptionBox()
                    {
                        EditorHelpBox mainDescription = new EditorHelpBox(
                        "Add prefab links to list and click 'Apply' button to refresh " +
                        "hierarchy create menu. After that you can create zones by right click " +
                        "-> Zones -> Your zone", MessageType.Info);

                        verticalLayout.Add(mainDescription);
                    }
                    DrawDescriptionBox();

                    void DrawCollection()
                    {
                        IMGUIContainer container = new IMGUIContainer(() =>
                        {
                            EditorGUILayout.PropertyField(collectionAsset.FindProperty("_prefabs"));
                            collectionAsset.ApplyModifiedProperties();
                        });
                        verticalLayout.Add(container);
                    }
                    DrawCollection();

                    void DrawApplyButton()
                    {
                        IMGUIContainer container = new IMGUIContainer(() =>
                        {
                            if (GUILayout.Button("Apply", GUILayout.Height(30)))
                            {
                                HierarchyCreatorWriter.UpdateCreatorClass();
                                Debug.Log("Changes applied");
                            }
                        });
                        verticalLayout.Add(container);
                    }
                    DrawApplyButton();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Number", "Some String" })
            };

            return provider;
        }

        private static ZoneTypesCollection GetCollectionAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ZoneTypesCollection>(COLLECTION_PATH);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ZoneTypesCollection>();
                AssetDatabase.CreateAsset(asset, COLLECTION_PATH);
                AssetDatabase.SaveAssets();

                Debug.Log($"<b>Zones:</b> Zones collection asset created at path: {COLLECTION_PATH}");
            }

            return asset;
        }
    }

    internal class HorizontalLayout : VisualElement
    {
        public HorizontalLayout()
        {
            name = nameof(HorizontalLayout);
            style.flexDirection = FlexDirection.Row;
            style.flexGrow = 1;
        }
    }

    internal class VerticalLayout : VisualElement
    {
        public VerticalLayout()
        {
            name = nameof(VerticalLayout);
            style.flexDirection = FlexDirection.Column;
            style.flexGrow = 1;
        }
    }

    internal class ItemGroup : VisualElement
    {
        public ItemGroup()
        {
            name = nameof(ItemGroup);
            style.marginBottom = 10;
            style.backgroundColor = new StyleColor(new Color(0.20f, 0.20f, 0.20f));
            style.borderBottomColor = Color.red;
            style.flexDirection = FlexDirection.Column;
            style.flexGrow = 1;
        }
    }

    internal class HeaderLabel : Label
    {
        public HeaderLabel(string text) : base(text)
        {
            name = nameof(HeaderLabel);
            style.fontSize = 18;
            style.unityFontStyleAndWeight = FontStyle.Bold;
            style.marginBottom = 10;
        }
    }

    internal class EditorHelpBox : VisualElement
    {
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string label = "";

        public EditorHelpBox(string text, MessageType messageType, bool wide = true)
        {
            style.marginLeft = style.marginRight = style.marginTop = style.marginBottom = 4;
            Label = text;

            IMGUIContainer iMGUIContainer = new IMGUIContainer(() => { EditorGUILayout.HelpBox(label, messageType, wide); });

            iMGUIContainer.name = nameof(IMGUIContainer);
            Add(iMGUIContainer);
        }
    }
}