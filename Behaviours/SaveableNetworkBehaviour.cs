using Unity.Netcode;

namespace LvLSystemLC.Behaviours
{
    public abstract class SaveableNetworkBehaviour : NetworkBehaviour
    {
        public int uniqueId = 0;

        public abstract void SaveObjectData();

        public abstract void LoadObjectData();
    }
}