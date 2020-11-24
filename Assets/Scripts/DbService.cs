using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class DbService : BasicDbService {
    public DbService(string dbName) : base(dbName) {        
    }

    public void createTable<T>() {        
        mConnection.CreateTable<T>();
    }    

    public List<T> getList<T>() where T : new() {
        return mConnection.Table<T>().ToList();
    }

    public void insert<T>(T data) {
        mConnection.Insert(data);
    }

    public void insertOrReplace<T>(T data) {
        mConnection.InsertOrReplace(data);
    }

    public void insertAll<T>(List<T> dataList) {
        mConnection.InsertAll(dataList);
    }   
}
