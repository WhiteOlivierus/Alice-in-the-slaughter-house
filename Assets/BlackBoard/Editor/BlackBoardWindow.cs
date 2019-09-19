using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BB
{
    class BlackBoardWindow : EditorWindow
    {
        private static GUIStyle ToggleButtonStyleNormal = null;
        private static GUIStyle ToggleButtonStyleToggled = null;
        private bool _scene = true;
        private bool _project = false;

        private int scriptsIndex = 0;

        private Type[] types = new Type[] { typeof(float), typeof(int), typeof(string) };
        private string[] typesText = new string[] { "float", "int", "string" };

        private List<bool> showScriptsPosition = new List<bool>();
        private Dictionary<string, BlackBoardParameter> scriptData = new Dictionary<string, BlackBoardParameter>();

        private GUILayoutOption[] squareSize = new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) };

        // private bool showObjectPosition = true;
        // private string objectName = "GameObject";

        [MenuItem("Window/BlackBoard")]
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(BlackBoardWindow), false, "BlackBoard");
            window.minSize = new Vector2(400, 250);
            window.Repaint();
        }

        private void Reload()
        {
            BlackBoardManager.LoadData();

            if (scriptData.Count > 0)
            {
                return;
            }

            string[] scriptInstances = BlackBoardManager.GetScriptInstances();
            bool scriptInitialized = showScriptsPosition == default;

            for (int i = 0; i < scriptInstances.Length; i++)
            {
                try
                {
                    if (!scriptInitialized)
                    {
                        showScriptsPosition.Add(false);
                    }

                    scriptData.Add(scriptInstances[i], new BlackBoardParameter("Parameter_name"));
                }
                catch
                {
                    continue;
                }
            }
        }

        private void OnGUI()
        {
            Reload();

            GuiStateButtons();

            if (_project)
            {
                GuiProject();
            }
            else
            {
                // GuiScene();
                GUILayout.Label("Comming soon");
            }
        }

        private void GuiStateButtons()
        {
            GUILayout.BeginHorizontal();
            if (ToggleButtonStyleNormal == null)
            {
                ToggleButtonStyleNormal = "Button";
                ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
                ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
            }

            if (GUILayout.Button("Scene", _scene ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
            {
                _scene = true;
                _project = false;
            }

            if (GUILayout.Button("Project", _project ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
            {
                _scene = false;
                _project = true;
            }
            GUILayout.EndHorizontal();
        }

        private void GuiProject()
        {
            GuiAddScriptBlock();
            GuiLine();

            List<string> scriptNames = scriptData.Keys.ToList();

            for (int i = 0; i < scriptNames.Count; i++)
            {
                string keys = scriptNames[i];
                showScriptsPosition[i] = EditorGUILayout.Foldout(showScriptsPosition[i], keys);

                if (showScriptsPosition[i])
                {
                    GuiCreateParameterBlock(keys);
                    GuiLine();

                    BlackBoardParameter[] bbps = BlackBoardManager.GetScriptParameters(keys);
                    if (bbps != null)
                    {
                        for (int j = 0; j < bbps.Length; j++)
                        {
                            BlackBoardParameter bbp = GuiAllParametersBlock(keys, bbps[j]);
                            if (bbp.value != null)
                            {
                                BlackBoardManager.UpdateParameter(keys, bbp, j);
                            }
                        }
                    }
                }
            }
        }

        // private void GuiScene()
        // {
        //     GuiLine();
        //     scriptsIndex = EditorGUILayout.Popup(scriptsIndex, BlackBoardManager.GetAllSceneScriptIntances());

        //     showScriptsPosition = EditorGUILayout.Foldout(showScriptsPosition, scriptNames);
        //     if (showScriptsPosition)
        //     {
        //         GuiCreateParameterBlock();
        //         GuiGameObjectBlock();
        //         EditorGUI.indentLevel--;
        //     }
        // }

        // private void GuiGameObjectBlock()
        // {
        //     GuiLine();
        //     showObjectPosition = EditorGUILayout.Foldout(showObjectPosition, objectName);
        //     if (showObjectPosition)
        //     {
        //         // GuiAllParameterBlock();
        //     }
        // }

        private void GuiAddScriptBlock()
        {
            GUILayout.BeginHorizontal();

            string[] displayedOptions = BlackBoardManager.GetAllNonExsitentProjectScriptInstances();
            scriptsIndex = EditorGUILayout.Popup(scriptsIndex, displayedOptions);

            if (GUILayout.Button("+", squareSize))
            {
                if (displayedOptions.Count() <= scriptsIndex) { return; }

                BlackBoardManager.CreateScriptInstance(displayedOptions[scriptsIndex]);
                scriptData.Add(displayedOptions[scriptsIndex], new BlackBoardParameter("Parameter_name"));
                showScriptsPosition.Add(false);
            }

            GUILayout.EndHorizontal();
        }

        private void GuiCreateParameterBlock(string key)
        {
            GUILayout.BeginHorizontal();
            EditorGUI.indentLevel++;
            BlackBoardParameter bbp = scriptData[key];
            bbp.typeIndex = EditorGUILayout.Popup(bbp.typeIndex, typesText);
            EditorGUI.indentLevel--;

            bbp.type = types[bbp.typeIndex];
            bbp.accessor = key.Replace(".cs", "");
            bbp.name = EditorGUILayout.TextField(bbp.name);
            bbp = ShowValueField(bbp);

            if (GUILayout.Button("+", squareSize))
            {
                if (BlackBoardManager.CreateParameterInstance(key, bbp))
                    bbp = new BlackBoardParameter("Parameter_name");
            }

            if (GUILayout.Button("-", squareSize))
            {
                BlackBoardManager.RemoveScriptInstance(key);
                scriptData.Remove(key);
                return;
            }

            scriptData[key] = bbp;

            GUILayout.EndHorizontal();
        }

        private BlackBoardParameter GuiAllParametersBlock(string key, BlackBoardParameter bbp)
        {
            GUILayout.BeginHorizontal();
            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(bbp.name, GUILayout.MaxWidth(113));
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            bbp = ShowValueField(bbp);

            if (GUILayout.Button("Remove"))
            {
                BlackBoardManager.RemoveParameterInstance(key, bbp);
                return default;
            }

            GUILayout.EndHorizontal();
            return bbp;
        }

        private static BlackBoardParameter ShowValueField(BlackBoardParameter bbp)
        {
            var value = bbp.value;
            if (bbp.value == null || bbp.value.GetType() != bbp.type)
            {
                try
                {
                    value = Activator.CreateInstance(bbp.type);
                }
                catch
                {
                    value = "";
                }
            }

            switch (bbp.type.ToString())
            {
                case "System.Int32":
                    bbp.value = EditorGUILayout.IntField(value);
                    break;
                case "System.Single":
                    bbp.value = EditorGUILayout.FloatField(value);
                    break;
                case "System.String":
                    bbp.value = EditorGUILayout.TextField(value);
                    break;
                default:
                    break;
            }
            return bbp;
        }

        private void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;

            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }
    }
}