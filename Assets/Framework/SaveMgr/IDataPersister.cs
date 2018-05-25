
public interface IDataPersister
{
    string Key
    {
        get;
    }

    object SaveData();

    void LoadData(object data);

    bool IsDirty();
}
