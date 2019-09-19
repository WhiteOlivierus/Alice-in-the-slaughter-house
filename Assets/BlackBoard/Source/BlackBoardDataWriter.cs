using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BB
{
    public static class BlackBoardDataWriter
    {
        public static T Read<T>(string path)
        {
            DoesDataExist(path);

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                T data;
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    data = (T)formatter.Deserialize(fileStream);
                }
                catch (SerializationException exception)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + exception.Message);
                    throw;
                }
                finally
                {
                    fileStream.Close();
                }
                return data;
            }
        }

        public static void Write<T>(T data, string path)
        {
            DoesDataExist(path);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, data);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private static void DoesDataExist(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }
    }
}