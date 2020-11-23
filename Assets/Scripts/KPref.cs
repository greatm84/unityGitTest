using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KPref : BasicDbService {
    private const string DB_NAME = "prefDb.db";
    private static KPref instance;

    public static KPref Inst() {
        if (instance == null) {
            instance = new KPref(DB_NAME);
        }
        return instance;
    }

    private KPref(string databaseName) : base(databaseName) {
        Debug.Log("Kpref Con");
        createDb();
    }

    private void createDb() {
        mConnection.CreateTable<KPrefData>();
    }

    public void put(string key, bool data) {
        put(key, data ? "1" : "0");
    }

    public void put(string key, int data) {
        put(key, data.ToString());
    }

    public void put(string key, float data) {
        put(key, data.ToString());
    }

    public void putObject<T>(string key, T data) {
        put(key, JsonUtility.ToJson(data));
    }

    public void put(string key, string data) {
        instance.mConnection.InsertOrReplace(new KPrefData { Key = key, Data = data });
    }

    public bool? getBoolean(string key) {
        var data = getKeyData(key);
        if (data == null) return null;
        return data.Data == "1";
    }

    public int? getInt(string key) {
        var data = getKeyData(key);
        if (data == null) return null;
        return int.Parse(data.Data);
    }

    public float? getFloat(string key) {
        var data = getKeyData(key);
        if (data == null) return null;
        return float.Parse(data.Data);
    }

    public string getString(string key) {
        var data = getKeyData(key);
        if (data == null) return null;
        return data.Data;
    }

    public T getObject<T>(string key) {
        var data = getKeyData(key);
        if (data == null) return default(T);
        return JsonUtility.FromJson<T>(data.Data);
    }

    private KPrefData getKeyData(string key) {
        return instance.mConnection.Table<KPrefData>().Where(x => x.Key == key).First();
    }

    public bool hasKey(string key) {
        return getKeyData(key) != null;
    }
}
