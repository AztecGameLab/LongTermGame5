using UnityEngine;

namespace SaveSystem
{
    public class SavePoint : Interactable
    {
        public override void OnInteract()
        {
            SaveLoad.SaveCurrentScene();
            print("saving...");
            base.OnInteract();
        }
    }
}
