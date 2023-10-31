using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    /* Muista tarkistaa, onko Unityn methodien per‰ss‰ olevaa 2D merkint‰‰, jos ei ole!!!
     * Rupee vituttamaan, ku menee p‰ivi‰ hukkaan ongelmien ettimisess‰ ja sit se ongelma on se,
     * ett‰ k‰ytin OnCollisionEnter/OnTriggerEnter ja olis pit‰nny olla 2D per‰ss‰ viel.
     * Eli OnCollisionEnter2D ja OnTriggerEnter2D. Perkele, ett‰ on pienest‰ kiinni.
     * ja kaikki ohjeet on tehty perustana 3D jutuille. Muista n‰‰ jutut perkele!!!
     * Never again.
     */
    public static GameManager gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        
        if(GUI.Button(new Rect(10, 140, 100, 30), "Save"))
        {
            Save();
        }
        if(GUI.Button(new Rect(10, 170, 100, 30), "Load"))
        {
            Load();
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");

        PlayerData data = new PlayerData();
        data.playerHealth = PlayerController.playerHealth;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Save Succesful");
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            PlayerController.playerHealth = data.playerHealth;
            Debug.Log("Load Succesful");

        }
    }
}
[Serializable]
class PlayerData
{
    public float playerHealth;
}