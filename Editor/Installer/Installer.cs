using UnityEditor;


namespace SimpleMan.Utilities.Editor
{
    [InitializeOnLoad]
    internal static class Installer
    {
        //------FIELDS
        public static string MAIN_FOLDER_PATH = "Assets/Plugins/SimpleMan/Zones";
        public static string MAIN_PACKAGE_PATH = "Assets/Plugins/SimpleMan/Zones/Editor/Installer/MainPackage.unitypackage";
        public static string SIMPLE_MAN_UTILITIES_PATH = "Assets/Plugins/SimpleMan/Utilities";
        public static string ODIN_INSPECTOR_PATH = "Assets/Plugins/Sirenix/Odin Inspector";




        //------METHODS
        static Installer()
        {
            if(!IsAssetAlreadyImported())
                InstallerWindow.Init();
        }

        public static void Install()
        {
            AssetDatabase.ImportPackage(MAIN_PACKAGE_PATH, true);
        }

        public static bool IsAssetAlreadyImported()
        {
            string[] subfolders = AssetDatabase.GetSubFolders(MAIN_FOLDER_PATH);
            return subfolders.Length > 1;
        }

        public static bool IsOdinExist()
        {
            return IsDependencyExist(ODIN_INSPECTOR_PATH);
        }

        public static bool IsUtilitiesExist()
        {
            return IsDependencyExist(SIMPLE_MAN_UTILITIES_PATH);
        }

        public static bool IsDependencyExist(string path)
        {
            string[] subfolders = AssetDatabase.GetSubFolders(path);
            return subfolders.Length > 1;
        }
    }
}