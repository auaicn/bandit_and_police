#if !UNITY_2019_2_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace Rokoko.MotionLibraryCompatibility
{
    internal class MotionLibraryWindowCompatibility : EditorWindow
    {        
        internal class Fonts
        {
            public static Font normalText = (Font)Resources.Load("Fonts/ProximaNova/Regular/proximanova-regular-webfont");
            public static Font boldText = (Font)Resources.Load("Fonts/ProximaNova/Bold/proximanova-bold-webfont");
        }

        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(MotionLibraryWindowCompatibility));
        }

        private void OnEnable()
        {
            minSize = new Vector2(650, 180);
        }

        void OnGUI()
        {            
            // Bold text style
            var boldText = new GUIStyle(GUI.skin.label);
            boldText.alignment = TextAnchor.MiddleCenter;
            boldText.font = Fonts.boldText;
            boldText.fontSize = 16;

            // Normal text style
            var centeredText = new GUIStyle(GUI.skin.label);
            centeredText.font = Fonts.normalText;
            centeredText.wordWrap = true;
            centeredText.alignment = TextAnchor.MiddleCenter;
            centeredText.fontSize = 16;
            
            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fixedWidth = 260;
            buttonStyle.fixedHeight = 50;

            GUILayout.FlexibleSpace();
            GUILayout.Space(10);
            GUILayout.Label("Motion Library needs different fuel", boldText);
            GUILayout.Space(25);
            
            GUILayout.Label("You are currently using an older version of .NET, however Motion Library requires you to use .NET API version 4.X. You can easily switch by clicking the button below.", centeredText);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            // Runtime version needs change and restart is required
            if (GUILayout.Button("Please change the setting and close Unity", buttonStyle))
            {
                if (EditorUtility.DisplayDialog("You need to set the Scripting Runtime Version to .NET 4.x Equivalent", "The Motion Library plugin can do this for you, but remember you need to manually re-open Unity and the project afterwards. Do you want to proceed?", "Yes, change it and close Unity", "No, not right now"))
                {
                    Close();
                    PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Latest;
                    EditorApplication.Exit(0);
                }
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
    }
}
#endif
