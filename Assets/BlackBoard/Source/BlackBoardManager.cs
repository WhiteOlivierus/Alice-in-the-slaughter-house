using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BB
{
    public static class BlackBoardManager
    {
        private const string scriptsLocationPath = "Assets/Scripts/";
        private static string blackBoardBinaryDataPath
        {
            get {
                if (Application.isEditor)
                {
                    return "Assets/StreamingAssets/BlackBoardCache.dat";
                }
                return Application.streamingAssetsPath + "/BlackBoardCache.dat";
            }
            set { }
        }

        private static Dictionary<string, List<BlackBoardParameter>> scriptInstances = new Dictionary<string, List<BlackBoardParameter>>();

        public static T GetParameterValue<T>(string key, string parameterName)
        {
            LoadData();

            if (!scriptInstances.ContainsKey(key + ".cs"))
            {
                throw new System.ArgumentNullException($@"Blackboard parameter " + parameterName + " does not exist in this context.");
            }

            List<BlackBoardParameter> values = scriptInstances[key + ".cs"];

            foreach (BlackBoardParameter value in values)
            {
                if (value.name == parameterName)
                {
                    return (T)value.value;
                }
            }

            return default;
        }

        public static void CreateScriptInstance(string key)
        {
            if (scriptInstances.ContainsKey(key))
            {
                Debug.LogWarning("Script you are trying to add is allready there.");
                return;
            }

            UpdateScriptInstance(key);
        }

        public static void RemoveScriptInstance(string key)
        {
            UpdateScriptInstance(key, true);
        }

        public static bool CreateParameterInstance(string key, BlackBoardParameter bbp)
        {
            if (Regex.IsMatch(bbp.name, @"[\d\W\0]+"))
            {
                Debug.LogWarning("This is not a good parameter name. Don't use numbers or special characters.");
                return false;
            }

            foreach (string currentKey in scriptInstances.Keys)
            {
                foreach (BlackBoardParameter tempbbp in scriptInstances[currentKey])
                {
                    if (tempbbp.name == bbp.name)
                    {
                        Debug.LogWarning("A parameter with this name allready exists. Make your parameter name more discriptive.");
                        return false;
                    }
                }
            }

            ChangeParameter(key, bbp);

            return true;
        }

        public static void UpdateParameter(string key, BlackBoardParameter bbp, int index)
        {
            try
            {
                scriptInstances[key][index] = bbp;
                BlackBoardDataWriter.Write<Dictionary<string, List<BlackBoardParameter>>>(scriptInstances, blackBoardBinaryDataPath);
            }
            catch
            {

            }
        }

        public static void RemoveParameterInstance(string key, BlackBoardParameter bbp)
        {
            ChangeParameter(key, bbp, true);
        }

        private static void UpdateScriptInstance(string key, bool remove = false)
        {
            if (!remove)
                scriptInstances.Add(key, new List<BlackBoardParameter>());
            else
                scriptInstances.Remove(key);

            UpdateData();
        }

        private static void ChangeParameter(string key, BlackBoardParameter bbp, bool remove = false)
        {
            if (!remove)
                scriptInstances[key].Add(bbp);
            else
                scriptInstances[key].Remove(bbp);

            UpdateData();
        }

        private static void UpdateData()
        {
            BlackBoardDataWriter.Write<Dictionary<string, List<BlackBoardParameter>>>(scriptInstances, blackBoardBinaryDataPath);
            BlackBoardUpdater.UpdateBlackBoard();
        }

        public static void LoadData()
        {
            if (scriptInstances.Count < 1)
            {
                scriptInstances = BlackBoardDataWriter.Read<Dictionary<string, List<BlackBoardParameter>>>(blackBoardBinaryDataPath);
            }
        }

        public static BlackBoardParameter[] GetScriptParameters(string key)
        {
            if (!scriptInstances.ContainsKey(key)) { return default; }
            return scriptInstances[key].ToArray();
        }

        public static string[] GetScriptInstances()
        {
            return scriptInstances.Keys.ToArray();
        }

        public static string[] GetAllProjectScriptIntances()
        {
            string[] scriptPaths = GetScriptPaths(scriptsLocationPath);
            return GetScriptNames(scriptPaths);
        }

        public static string[] GetAllNonExsitentProjectScriptInstances()
        {
            List<string> scriptNames = GetAllProjectScriptIntances().ToList();

            foreach (string key in scriptInstances.Keys)
            {
                try
                {
                    scriptNames.Remove(key);
                }
                catch
                {
                    continue;
                }
            }

            return scriptNames.ToArray();
        }

        private static string[] GetScriptNames(string[] scriptPaths)
        {
            List<string> scriptNames = new List<string>();
            Regex regex = new Regex(@"^(.+\/)*");

            foreach (string scriptPath in scriptPaths)
            {
                string s = regex.Replace(scriptPath, "");
                scriptNames.Add(s);
            }

            return scriptNames.ToArray();
        }

        private static string[] GetScriptPaths(string scriptLocationPath)
        {
            string[] scriptPaths = new string[1];

            if (!Directory.Exists(scriptLocationPath))
            {
                scriptPaths[0] = "No scripts found";
                return scriptPaths;
            }

            scriptPaths = Directory.GetFiles(scriptLocationPath, "*.cs", SearchOption.AllDirectories);

            return scriptPaths;
        }
    }
}
