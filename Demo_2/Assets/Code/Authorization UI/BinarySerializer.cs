using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySerializer
{
    private const string path = "/GameData.dat";

    private Hashtable data;

    public void SaveData()
    {
        if (data == null)
        {
            Debug.Log("Data is null, load data before saving");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path);

        try
        {
            bf.Serialize(file, data);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to serialize. Reason: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
            try
            {
                data = (Hashtable)bf.Deserialize(file);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                file.Close();
            }
        }
        else
        {
            Debug.Log("Not found file. Create New.");
            SetDefaultData();
            SaveData();
        }

    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            try
            {
                File.Delete(Application.persistentDataPath + path);
            }
            catch
            {
                Debug.Log("C удалением файла что то пошло не так");
            }
            SetDefaultData();
            SaveData();
        }
    }


    public ref Hashtable GetData()
    {
        return ref data;
    }

    public void SetDefaultData()
    {
        data = new Hashtable()
        {
            {"login", ""},
            {"password", ""}
        };

    }
}
