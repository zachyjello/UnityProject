﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    interface IDamageable
    {
        public void ReceiveDamage(int Damage);

        public void OnDeath();
    }
}
