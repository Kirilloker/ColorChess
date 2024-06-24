using ColorChessModel;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySerializer
{
    //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/GameData.dat");
    string filePath = Application.persistentDataPath + "/GameData.dat";

    private Hashtable data;

    public void SaveData()
    {
        if (data == null)
        {
            Print.Log("Data is null, load data before saving");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        try
        {
            bf.Serialize(file, data);
        }
        catch (SerializationException e)
        {
            Print.Log("Failed to serialize. Reason: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            try
            {
                data = (Hashtable)bf.Deserialize(file);
            }
            catch (SerializationException e)
            {
                Print.Log("Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                file.Close();
            }
        }
        else
        {
            Print.Log("Not found file. Create New.");
            SetDefaultData();
            SaveData();
        }

    }

    public void ResetData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                Print.Log("C ��������� ����� ��� �� ����� �� ���");
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
