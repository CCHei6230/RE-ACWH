using System.Collections;
using UnityEngine;
namespace CustomInterfaces
{
    public interface iDamagable
    {
        void TakeDamage( int _damage);
        void Death();
    }
    //------------------------------------------------------------------------------------------------------------
    public interface iCanBeLockOn
    {
        public SpriteRenderer Sprite { get; }
        public void BeLockOn(out SpriteRenderer _Sprite ,out GameObject _RootObj);
    }
}