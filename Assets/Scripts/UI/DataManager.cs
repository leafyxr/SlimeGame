using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour {
    public SaveData data;
    [SerializeField]
    private SceneLoader LoadingScreen;

	void Awake () {
        if (!dataLoad())
        {
            dataSave();
        }
	}

    public bool dataLoad()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        data = (SaveData)bf.Deserialize(file);
        file.Close();
        Debug.Log("Game Loaded");
        return true;
    }

    public void dataSave()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");
        return;
    }

    public void deleteData()
    {
        File.Delete(Application.persistentDataPath + "/save.dat");
    }
}
