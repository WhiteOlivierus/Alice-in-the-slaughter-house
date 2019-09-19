using System;
using System.Diagnostics;

namespace BB
{
    public class BlackBoardType<T>
    {
        private const int stackIndex = 4;

        private Type accessableType;
        private string parameterName;

        public T value
        {
            get
            {
                return TryGetValue();
            }
            private set { }
        }

        public BlackBoardType(string name, Type accessableType)
        {
            this.parameterName = name;
            this.accessableType = accessableType;
        }

        public T GetValue()
        {
            return value;
        }

        private Type GetCallingType(StackTrace stackTrace)
        {
            return stackTrace.GetFrame(stackIndex).GetMethod().ReflectedType;
        }

        private bool MayReturn(Type accessableType, Type accessingType)
        {
            return accessableType == accessingType;
        }

        private T TryGetValue()
        {
            StackTrace stackTractObject = new StackTrace();

            Type accessingType = GetCallingType(stackTractObject);

            if (!MayReturn(accessableType, accessingType))
            {
                throw new System.ArgumentException("BlackBoard parameter: " + parameterName + " not accesable by " + accessingType.ToString());
            }
            return BlackBoardManager.GetParameterValue<T>(accessableType.ToString(), parameterName);
        }
    }
}