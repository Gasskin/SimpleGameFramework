//==========================================
// 这个文件是自动生成的...
// 生成日期：2021年3月25日10点38分
//
//
//==========================================


public struct UIStruct
{
    public string name;
    public string path;
    public UIStruct(string name,string path)
    {
        this.name = name;
        this.path = path;
    }
}


public static class UIs
{
    public static UIStruct TestUI = new UIStruct("TestUI","UI/TestUI");
    public static UIStruct Fixed1 = new UIStruct("Fixed1","UI/Fixed1");
    public static UIStruct Fixed2 = new UIStruct("Fixed2","UI/Fixed2");
    public static UIStruct Normal1 = new UIStruct("Normal1","UI/Normal1");
    public static UIStruct Normal2 = new UIStruct("Normal2","UI/Normal2");
    public static UIStruct Pop1 = new UIStruct("Pop1","UI/Pop1");
    public static UIStruct Pop2 = new UIStruct("Pop2","UI/Pop2");
    public static UIStruct Update1 = new UIStruct("Update1","UI/Update1");
    public static UIStruct Update2 = new UIStruct("Update2","UI/Update2");
}
