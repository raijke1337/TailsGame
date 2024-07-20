namespace Arcatech.Items
{
    public interface IWeapon : IUsable
    {
        public IDrawItemStrategy DrawStrategy { get; }
        public IWeaponUseStrategy UseStrategy { get; }

    }

   
}