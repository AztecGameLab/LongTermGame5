
//Interface that classes/components need to implement if they want to be saved
public interface ISaveableComponent
{
    SaveData GatherSaveData();
    void RestoreSaveData(SaveData state);
}
