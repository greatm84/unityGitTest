﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manmove : MonoBehaviour {
    InputField inputKey;
    InputField inputData;
    Button btnSave;

    InputField inputKey2;
    Button btnQuery;
    Text txtResult;


    // Start is called before the first frame update
    void Start() {
        findObjects();
    }

    // Update is called once per frame
    void Update() {

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
        if (key.Length == 0) {
            Debug.Log("no key");
            return;
        }

        var service = new DbService("TempDb");
        service.createTable<Animal>();

        service.insert(new Animal() {Cid = 1000, Name = "Babo" });
    }

    public void CallButtonQuery() {
        var key = inputKey2.text;
        if (key.Length == 0) {
            Debug.Log("no key2");
            return;
        }
        /*
        var data = KPref.Inst().getString(key);
        txtResult.text = data;
        

        var data = KPref.getObject<People>(key);
        txtResult.text = data.ToString();
        */

        var service = new DbService("TempDb");
        service.createTable<Animal>();

        var data = service.getList<Animal>();
        
        foreach(var anim in data) {
            Debug.Log($"{anim.Cid} {anim.Name}");
        }
    }

    public class Animal {
        public int Cid { get; set; }

        public string Name { get; set; }

        public override string ToString() {
            return string.Format($"[Animal: Cid={Cid}, Name={Name}]");
        }
    }

    class People{
        public int Id;
        public string Name;
        public int Strength;
        public int Intelligent;
        public int Dex;

        public People(int id, string name, int strength, int intelligent, int dex) {
            Id = id;
            Name = name;
            Strength = strength;
            Intelligent = intelligent;
            Dex = dex;
        }
    }
}
