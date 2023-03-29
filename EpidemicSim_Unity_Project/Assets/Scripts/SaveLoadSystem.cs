using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    public static void SaveData(VariableObject varObj) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/simulationData.SIMDAT";

        FileStream stream = new FileStream(path, FileMode.Create);

        VariableData varData = new VariableData(varObj);

        formatter.Serialize(stream, varData);
        stream.Close();
    }
    public static VariableData LoadData() 
    {
        string path = Application.persistentDataPath + "/simulationData.SIMDAT";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            VariableData data = formatter.Deserialize(stream) as VariableData;
            stream.Close();

            return data;
        }
        else 
        {
            Debug.LogError("Error: Save file doesn't exist in " + path);
            return null;
        }
    }
}
