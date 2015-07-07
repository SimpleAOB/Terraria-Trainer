using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria_Trainer
{
    class PlayerInfo
    {
        public class playerDataAddr
        {
            public int Health;
            public int MaxHealth;
            public int Mana;
            public int MaxMana;
            public int amslot;

            public playerDataAddr(int _maxHealth, int _health, int _maxMana, int _mana, int _amslot)
            {
                Health = _health;
                MaxHealth = _maxHealth;
                Mana = _mana;
                MaxMana = _maxMana;
                amslot = _amslot;
            }
        }
        public struct PlayerData
        {
            public int baseAddress;
            public int[] multilevel;
            public playerDataAddr offsets;
        }
    }
}
