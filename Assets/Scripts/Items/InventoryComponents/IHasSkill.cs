using Arcatech.Skills;

namespace Arcatech.Items
{
    /// <summary>
    /// for inventory components
    /// </summary>
    public interface IHasSkill
    {
        public ISkill GetSkill { get; }
    }

   
}