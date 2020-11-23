using SQLite4Unity3d;
public class KPrefData
{
    [PrimaryKey]
    public string Key { get; set; }

    public string Data { get; set; }    

    public override string ToString() {
        return string.Format($"[KPrefData: Key={Key}, Data={Data}]");
    }
}
