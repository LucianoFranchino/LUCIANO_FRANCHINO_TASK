using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Application.persistentDataPath + "/inventory_save.json";
        Debug.Log("Save file path: " + saveFilePath);
    }

    public void SaveInventory(InventoryData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Inventory Save!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error save inventory: " + e.Message);
        }
    }

    public InventoryData LoadInventory()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                InventoryData data = JsonUtility.FromJson<InventoryData>(json);
                Debug.Log("Inventory Load!");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error" + e.Message);
                return null;
            }
        }
        else
        {
            Debug.Log("File not exist");
            return null;
        }
    }

    public bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
    }

}
