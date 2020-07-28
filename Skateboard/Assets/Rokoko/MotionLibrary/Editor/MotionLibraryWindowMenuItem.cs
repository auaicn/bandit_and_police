using UnityEditor;

namespace Rokoko.MotionLibrary
{
    public class MotionLibraryWindowMenuItem
    {
        private const string PathToMotionLibraryDLL = "Assets/Rokoko/MotionLibrary/Plugins/MotionLibrary.dll";

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Window/Motion Library")]
        public static void ShowWindow()
        {
#if !UNITY_2019_2_OR_NEWER
            bool scriptingRuntimeNeedsUpgrade = PlayerSettings.scriptingRuntimeVersion != ScriptingRuntimeVersion.Latest;

            if (scriptingRuntimeNeedsUpgrade)
            {
                MotionLibraryCompatibility.MotionLibraryWindowCompatibility.ShowWindow();
                return;
            }
#endif
            InvokeMotionLibraryWindow();
        }

        private static void InvokeMotionLibraryWindow()
        {
            System.Reflection.Assembly MotionLibraryDLL = null;
            try
            {
                MotionLibraryDLL = System.Reflection.Assembly.LoadFile(PathToMotionLibraryDLL);
            }
            catch (System.IO.FileNotFoundException)
            {
                UnityEngine.Debug.LogError("Cannot find 'MotionLibrary.dll'. Please ensure that it has not been removed from its expected location at '" + PathToMotionLibraryDLL + "'");
            }

            if (MotionLibraryDLL != null)
            {
                System.Type MotionLibraryWindowType = MotionLibraryDLL.GetType("Rokoko.MotionLibrary.MotionLibraryWindow");
                System.Reflection.MethodInfo ShowWindowMethod = MotionLibraryWindowType.GetMethod("ShowWindow", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                ShowWindowMethod.Invoke(null, null);
            }
        }
    }
}
