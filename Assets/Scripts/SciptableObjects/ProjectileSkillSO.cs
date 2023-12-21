using Arcatech.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "New Projectile Skill Config", menuName = "Skills/Projectile Skill")]
    public class ProjectileSkillSO : SkillControlSettingsSO
    {
        public ProjectileConfiguration SkillProjectile;
    
    }
}
