public class HeroChamp : BaseChamp {

    public int Str { get; set; }
    public int Dex { get; set; }
    public int Con { get; set; }
    public int Intel { get; set; }
    public int Wis { get; set; }
    public int Cha { get; set; }

    public override string ToString() {
        return string.Format($"[HeroChamp:Id={Id},Name={Name},Str={Str},Dex={Dex},Con={Con},Intel={Intel},Wis={Wis},Cha={Cha}]");
    }    
}
