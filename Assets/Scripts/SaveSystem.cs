using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/*
public static class SaveSystem {
    public static void SaveData() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedata.dat");
        formatter.Serialize(file, CustomizationData.name);
        formatter.Serialize(file, CustomizationData.baseMaterial);
        file.Close();
    }

    public static void LoadData() {
        if (File.Exists(Application.persistentDataPath + "/savedata.dat")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedata.dat", FileMode.Open);
            CustomizationData.name = (string)formatter.Deserialize(file);
            CustomizationData.baseMaterial = (string)formatter.Deserialize(file);
            file.Close();
        } else {
            Debug.Log("Save file not found.");
        }
    }
}
*/
public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/savedata.dat";

    public static void SaveData(PlayerData data) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerData LoadData() {
        if (File.Exists(savePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        } else {
            Debug.LogError("Save file not found in " + savePath);
            return null;
        }
    }
}