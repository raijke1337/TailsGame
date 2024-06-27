namespace Arcatech.Items
{
    public interface IWeapon : IUsableItem, INeedsOwner
    {
        public IDrawItemStrategy DrawStrategy { get; }
        

    }

   
}