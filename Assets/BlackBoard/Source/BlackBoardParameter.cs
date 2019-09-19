using System;

namespace BB
{
    [Serializable]
    public struct BlackBoardParameter
    {
        public Type type;
        public int typeIndex;
        public string name;
        public string accessor;
        public dynamic value;

        public BlackBoardParameter(string name = "") : this()
        {
            this.name = name;
        }
    }
}