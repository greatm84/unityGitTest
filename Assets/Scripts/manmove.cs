using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manmove : MonoBehaviour
{
    InputField inputKey;
    InputField inputData;
    Button btnSave;

    InputField inputKey2;
    Button btnQuery;
    Text txtResult;    


    // Start is called before the first frame update
    void Start()
    {
        findObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void findObjects() {
        inputKey = GameObject.Find("inputKey").GetComponent<InputField>();
        inputData = GameObject.Find("inputData").GetComponent<InputField>();
        btnSave = GameObject.Find("btnSave").GetComponent<Button>();

        inputKey2 = GameObject.Find("inputKey2").GetComponent<InputField>();
        btnQuery = GameObject.Find("btnQuery").GetComponent<Button>();
        txtResult = GameObject.Find("txtResult").GetComponent<Text>();
    }

    public void CallButtonSave() {        
        var key = inputKey.text;
        if(key.Length == 0) {
            Debug.Log("no key");
            return;
        }
        var data = inputData.text;

        KPref.Inst().put(key, data);
        Debug.Log($"save comp {key}, {data}");
    }

    public void CallButtonQuery() {        
        var key = inputKey2.text;
        if (key.Length == 0) {
            Debug.Log("no key2");
            return;        
        }
        var data = KPref.Inst().getString(key);
        txtResult.text = data;
    }
}
