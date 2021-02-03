using BP.Units.Weapons;
using BP.Units;

public interface IDamageable
{
    Faction GetFaction();
    void TakeDmg(float dmg, DamageType dmgType);
}
