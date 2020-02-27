public class ScriptEngineParams
{ }

public class MouseClickParams : ScriptEngineParams
{
    public enum clicktype { single_click=0, double_click};
    public clicktype click { get; set; }
}

public class SelectWindowParams : ScriptEngineParams
{
    public string window { get; set; }
}

public class SetCursorParams : ScriptEngineParams
{
    public int x { get; set; }
    public int y { get; set; }
}

public class SendKeysParams : ScriptEngineParams
{
    public string str { get; set; }
}

