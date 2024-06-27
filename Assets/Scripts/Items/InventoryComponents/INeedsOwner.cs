using Arcatech.Units;

namespace Arcatech.Items
{
    public interface INeedsOwner : IHasOwner
    {
        public INeedsOwner SetOwner(DummyUnit owner);
    }

   
}