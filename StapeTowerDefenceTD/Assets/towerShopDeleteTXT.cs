using UnityEngine;
using TMPro;

public class towerShopDeleteTXT : MonoBehaviour
{

    [SerializeField] private TMP_Text txt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //txt.text.Remove(txt.text.Length-1) ;
        txt.text = txt.text.Replace('$', ' ').TrimEnd();
        print(txt.text);
    }

}
