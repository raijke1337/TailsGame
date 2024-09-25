using Arcatech.Skills;

namespace Arcatech.Items
{
    public interface IWeapon : IUsable, IAffectsItemDisplay
    {
        public IWeaponUseStrategy UseStrategy { get; }

    }

   
}