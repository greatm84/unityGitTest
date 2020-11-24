using SQLite4Unity3d;

public class BaseChamp {
    protected static string sQueryText = null;

    [PrimaryKey]
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString() {
        return string.Format($"[BaseChamp: Id={Id},Name={Name}]");
    }
}
