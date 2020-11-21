using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class KshDb {
    public enum DataType {
        INT = 0,
        REAL,
        TEXT,
        BLOB
    };

    public enum KeyType {
        NOTHING = 0,
        PRIMARYKEY,
        INDEX,
    };

    public class ColumnInfo {
        public KeyType keyType = KeyType.NOTHING;
        public DataType dataType;
        public string columnName;
        public int param;
        public ColumnInfo(string columnName, DataType dataType, int param = -1, KeyType keyType = KeyType.NOTHING) {
            this.columnName = columnName;
            this.dataType = dataType;
            this.keyType = keyType;
            this.param = param;
        }
    }

    public class TableInfo {
        public string tableName;
        public List<ColumnInfo> columnList = new List<ColumnInfo>();

        public TableInfo(string tableName) {
            this.tableName = tableName;
        }
    }

    private SQLiteConnection conn = null;
    public string DBName { get; }

    public KshDb(string dbName) {
        DBName = dbName;
        string dbFilePath = Application.dataPath + "/" + dbName;
        string dbPath = "URI=file:" + dbFilePath;
        if (!File.Exists(dbFilePath)) {
            SqliteConnection.CreateFile(dbFilePath);
        }

        conn = new SQLiteConnection(dbPath);
        conn.
    }

    ~KshDb() {
        if (conn != null) {
            conn.Close();
        }
    }

    public bool isDbConnected() {
        return conn.State == ConnectionState.Connecting;
    }

    public void createTable(TableInfo tableInfo) {
        if (tableInfo.tableName == null || tableInfo.tableName.Length == 0 || tableInfo.columnList.Count == 0) {
            throw new ArgumentException("not enough tableInfo");
        }
        string sql = "create table if not exists " + tableInfo.tableName + " (";
        string indexColumnStr = "";
        foreach (ColumnInfo info in tableInfo.columnList) {
            sql += info.columnName + " " + info.dataType.ToString();

            if (info.param != -1) {
                sql += "(" + info.param + ")";
            }

            switch (info.keyType) {
                case KeyType.INDEX:
                    indexColumnStr += (indexColumnStr.Length == 0) ? info.columnName : "," + info.columnName;
                    break;
                case KeyType.PRIMARYKEY:
                    sql += " primary key";
                    break;
            }
            sql += ",";
        }
        if (indexColumnStr.Length == 0) {
            sql = sql.Substring(0, sql.Length - 1);
            sql += ")";
        } else {
            sql += "index idx (" + indexColumnStr + "))";
        }
        new SqliteCommand(sql, conn).ExecuteNonQuery();
    }

    public void dropTable(string tableName) {
        string cmd = "drop table if exists " + tableName;
        new SqliteCommand(cmd, conn).ExecuteNonQuery();
    }

    public bool checkTable(string tableName) {
        string sql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='" + tableName + "'";
        object result = new SqliteCommand(sql, conn).ExecuteScalar();
        result = (result == DBNull.Value) ? null : result;
        int countDis = Convert.ToInt32(result);
        return countDis > 0;
    }

    public void checkTableColumns(TableInfo tableInfo) {
        if (tableInfo.tableName == null || tableInfo.tableName.Length == 0 || tableInfo.columnList.Count == 0) {
            throw new ArgumentException("not enough tableInfo");
        }
        {
            // Add column
            string sql = "alter table " + tableInfo.tableName + " ";
            foreach (ColumnInfo info in tableInfo.columnList) {
                sql += "ADD COLUMN IF NOT EXISTS " + info.columnName + " " + info.dataType.ToString();
                if (info.param != -1) {
                    sql += "(" + info.param + ")";
                }

                if (info.keyType == KeyType.PRIMARYKEY) {
                    sql += " primary key";
                }
                sql += ",";
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql += ";";
            new SqliteCommand(sql, conn).ExecuteNonQuery();
        }
        // check property
        {
            foreach (ColumnInfo info in tableInfo.columnList) {
                string sql = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS " +
                    "WHERE table_name = '" + tableInfo.tableName + "' AND COLUMN_NAME = '" + info.columnName + "'";
                string result = (string)new SqliteCommand(sql, conn).ExecuteScalar();
                if (!info.dataType.ToString().Equals(result, StringComparison.OrdinalIgnoreCase)) {
                    sql = "alter table " + tableInfo.tableName + " modify " + info.columnName + " " +
                        info.dataType.ToString();
                    if (info.param != -1) {
                        sql += "(" + info.param + ")";
                    }

                    if (info.keyType == KeyType.PRIMARYKEY) {
                        sql += " primary key";
                    }
                    new SqliteCommand(sql, conn).ExecuteNonQuery();
                }
            }
        }
    }

    public void deleteRow(string tableName, string where) {
        string cmd = "delete from " + tableName + " where " + where;
        new SqliteCommand(cmd, conn).ExecuteNonQuery();
    }

    public int insertValues(string tableName, params string[] values) {
        string cmd = "insert into " + tableName + " VALUES(";
        int i = 0;
        foreach (string value in values) {
            string cmdValue = value;
            bool isNum = int.TryParse(value, out i);
            if (!isNum) {
                cmdValue = "'" + value + "'";
            }
            cmd += cmdValue + ",";
        }
        cmd = cmd.Substring(0, cmd.Length - 1);
        cmd += ")";
        try {
            new SqliteCommand(cmd, conn).ExecuteNonQuery();
            return 0;
        } catch (SqliteException ex) {
            throw ex;
        }
    }

    public int replaceValues(string tableName, params string[] values) {
        string cmd = "replace into " + tableName + " VALUES(";
        int i = 0;
        foreach (string value in values) {
            string cmdValue = value;
            bool isNum = int.TryParse(value, out i);
            if (!isNum) {
                cmdValue = "'" + value + "'";
            }
            cmd += cmdValue + ",";
        }
        cmd = cmd.Substring(0, cmd.Length - 1);
        cmd += ")";
        try {
            return new SqliteCommand(cmd, conn).ExecuteNonQuery();
        } catch (SqliteException e) {
            throw e;
        }
    }

    public int getTableValueCount(string tableName, string column = null, string value = null) {
        string cmd = "select count(*) from " + tableName;
        if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(value)) {
            cmd += " where " + column + "=" + value;
        }
        object result = new SqliteCommand(cmd, conn).ExecuteScalar();
        result = (result == DBNull.Value) ? null : result;
        int countDis = Convert.ToInt32(result);
        return countDis;
    }

    public string getMaxValue(string tableName, string column) {
        string cmd = "select " + column + " from " + tableName + " order by " + column + " desc limit 1";
        string data = null;
        try {
            data = (string)new SqliteCommand(cmd, conn).ExecuteScalar();
        } catch (Exception e) {
            throw e;
        } finally {

        }
        return data;
    }

    public SqliteDataReader getDataReader(string query) {
        return new SqliteCommand(query, conn).ExecuteReader();
    }

    SqliteTransaction trans;

    public void beginTransaction() {
        if (trans == null) {
            trans = conn.BeginTransaction();
        }
    }

    public void commitTranscation() {
        if (trans != null && trans.Connection != null) {
            trans.Commit();
        }
    }

    public void rollbakTransaction() {
        if (trans != null) {
            try {
                trans.Rollback();
            } catch (SqliteException ex) {
                if (trans.Connection != null) {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }


    public void destroy() {
        if (conn != null) {
            conn.Close();
            conn = null;
        }
    }
}

