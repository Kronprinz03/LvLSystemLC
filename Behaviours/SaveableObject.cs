
namespace LvLSystemLC.Behaviours
{
    public abstract class SaveableObject : GrabbableObject
    {
        public int uniqueId;

        public override void LoadItemSaveData(int saveData)
        {
            base.LoadItemSaveData(saveData);

            uniqueId = saveData;
        }

        public override int GetItemDataToSave()
        {
            return uniqueId;
        }

        public virtual void Awake()
        {
            if (!IsHost)
            {
                return;
            }
            uniqueId = UnityEngine.Random.Range(0, 1000000);

            var saveableNetworkBehaviours = transform.GetComponentsInChildren<SaveableNetworkBehaviour>();

            foreach (var item in saveableNetworkBehaviours)
            {
                item.uniqueId = uniqueId;
            }
        }


        public abstract void SaveObjectData();

        public abstract void LoadObjectData();
    }
}