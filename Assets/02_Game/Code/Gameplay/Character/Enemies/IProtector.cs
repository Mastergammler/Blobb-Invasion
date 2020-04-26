using BlobbInvasion.Gameplay.Items.Crafting.Bullets;

namespace BlobbInvasion.Gameplay.Character.Enemies
{
    // Interaction for Items that protect enemies
    // Interactions are tailored for bullets that hit them
    // Bullets are the triggers, that will access the protector with this interface
    public interface IProtector
    {
        // Health of the protection item
        float Hitpoints { get; }
        // Dmg reduction for bullets that pierce it
        float DamageReductionMod { get; }
        // The reduction of the speed of the flying bullet
        // This is only importont for piercing bullets
        float SpeedReductionMod { get; }
        // Function to call from the bullets that hit it
        void BulletHit(float damage, IBullet bullet);
    }
}