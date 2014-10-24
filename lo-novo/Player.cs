using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lo_novo
{
    public enum Traits
    {
        // physical
        MainStrength,
        MainDexterity,
        MainCharisma,
        MainIntelligence,
        MainSpeed,

        // weapon skill
        WeaponBlade, // SPD cap
        WeaponBlaster, // DEX cap
        WeaponBlunt, // STR cap
        WeaponWrestling, // STR cap

        // thinking skillz
        SkillBio, // INT cap
        SkillEvade, // Max(DEX, SPD) cap
        SkillPunt, // DEX
        SkillDefend, // STR cap
        SkillObservant, // Max(INT, SPD) cap
        SkillLore, // INT cap
        SkillDeduction, // INT cap
        SkillCharm, // CHA cap
        SkillIntimidate, // Min(STR, CHA) cap
        SkillPersuasion, // Min(INT, CHA) cap
        SkillRepair, // Max(DEX, INT)
        SkillWeaponize, // Max(CHA, INT)

        // secret, magic attributes
        MagicTenacity,
        MagicPower,
        MagicControl,
        MagicLuck,
        MagicTaint,

        // personality tracking
        PersonalityHonest,
        PersonalityAnnoying,
        PersonalityAmenable,
        PersonalityReckless
    }

    public class Player : INoun
    {
        public string Name;
        public List<string> Aliases = new List<string>();
        public IIRC IRC;
        public Room Room;
        public List<Thing> Inventory = new List<Thing>();
        public DefaultInventoryResponses DefaultInventoryResponses;

        public int Level;
        public int LevelUpAvailable;
        public Dictionary<Traits, int> Traits;

        public string Appearance;

        public void Tick() { }

        public override string ToString()
        {
            return Name;
        }
    }
}
