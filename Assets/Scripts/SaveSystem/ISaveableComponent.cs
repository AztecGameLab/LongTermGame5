namespace SaveSystem
{
    //Interface that classes/components need to implement if they want to be saved
    public interface ISaveableComponent
    {
        ISaveData GatherSaveData();
        void RestoreSaveData(ISaveData state);
    }
}