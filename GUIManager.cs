using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    // Start is called before the first frame update
    void Start()
    {
        infoText.SetText("");
    }

    public void setInfoText(string s)
    {
        infoText.SetText(s);
    }
    public void nullText()
    {
        StopCoroutine(disableText());
        StartCoroutine(disableText());
    }
    IEnumerator disableText()
    {
        yield return new WaitForSeconds(2f);
        infoText.SetText("");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
