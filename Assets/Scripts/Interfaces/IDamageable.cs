using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    interface IDamageable
    {
        public abstract void ReceiveDamage(float Damage);

        public abstract void OnDeath();
    }
}
