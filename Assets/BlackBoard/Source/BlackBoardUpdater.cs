using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BB
{
    public static class BlackBoardUpdater
    {
        private const string blackBoardPath = "Assets/BlackBoard/BlackBoard.cs";
        private const string blackBoardBasePath = "Assets/BlackBoard/Data/BlackBoardBase.txt";
        private const string blackBoardParameterPath = "Assets/BlackBoard/Data/BlackBoardParameterTemplate.txt";

        private static List<string> blackBoardLines = new List<string>();
        private static int parameterBeginIndex;

        public static void UpdateBlackBoard()
        {
            LoadBlackBoard();
            if (Application.isEditor)
                SaveBlackBoard();
            #if UNITY_EDITOR
            AssetDatabase.ImportAsset(blackBoardPath);
            #endif
        }

        private static void LoadBlackBoard()
        {
            blackBoardLines.Clear();

            FileStream blackBoardBaseFileStream = new FileStream(blackBoardBasePath, FileMode.Open, FileAccess.Read);
            using (StreamReader baseBlackBoard = new StreamReader(blackBoardBaseFileStream))
            {
                string line;
                while ((line = baseBlackBoard.ReadLine()) != null)
                {
                    blackBoardLines.Add(line);

                    if (blackBoardLines.Last().Contains("{"))
                    {
                        parameterBeginIndex = blackBoardLines.IndexOf(blackBoardLines.Last());
                    }
                }
            }
        }

        private static void SaveBlackBoard()
        {
            List<string> allParameters = GetBlackBoardParameters();

            blackBoardLines.InsertRange(parameterBeginIndex + 1, allParameters.ToList());

            using (StreamWriter newBlackBoard = new StreamWriter(blackBoardPath, false))
            {
                foreach (string line in blackBoardLines)
                {
                    newBlackBoard.WriteLine(line);
                }
            }
        }

        private static List<string> GetBlackBoardParameters()
        {
            List<string> allParameters = new List<string>();
            string[] scriptInstances = BlackBoardManager.GetAllProjectScriptIntances();

            for (int i = 0; i < scriptInstances.Length; i++)
            {
                BlackBoardParameter[] blackBoardParams = BlackBoardManager.GetScriptParameters(scriptInstances[i]);

                if (blackBoardParams != null)
                {
                    foreach (BlackBoardParameter bbp in blackBoardParams)
                    {
                        allParameters.Add(CreateParameter(bbp));
                    }
                }
            }

            return allParameters;
        }

        private static string CreateParameter(BlackBoardParameter bbp)
        {
            string template = LoadTemplate();

            string type = bbp.type.ToString();
            string _name = "_" + bbp.name;
            string mono = bbp.accessor;
            string name = bbp.name;

            return string.Format(template, type, _name, mono, name);
        }

        private static string LoadTemplate()
        {
            return File.ReadAllText(blackBoardParameterPath);
        }
    }
}