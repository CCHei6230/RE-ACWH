using System.Collections;
using UnityEngine;
namespace CustomInterfaces
{
    public interface iDamagable
    {
        public int HP { get; set; }
        void TakeDamage( int _damage);
    }
    //------------------------------------------------------------------------------------------------------------
    public interface iCanBeLockOn
    {
        public SpriteRenderer Sprite { get; }
        public void BeLockOn(out SpriteRenderer _Sprite ,out GameObject _RootObj);
    }
}