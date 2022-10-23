using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivaleHelper.Enums
{
    public static class AOZScore
    {
        //List for reference and for looping through all AOZ Targets for debugging when a specific target isn't available on a given week
        public static List<byte> AOZTargets = new()
        {
            1,  //Too Fast, Too Furious
            2,  //Slow and Steady
            3,  //10 Ways to Die
            4,  //4 Ways to Die
            5,  //Can't Touch This
            6,  //Ain't Got Time to Bleed
            7,  //Modus Interruptus
            8,  //A Walk in the Park
            9,  //Mastery of Fire
            10, //Mastery of Water
            11, //Mastery of Earth
            12, //Mastery of Wind
            13, //Mastery of Ice
            14, //Mastery of Lightning
            15, //Elemental Mastery
            17, //Enfeeble Me Tender
            18, //Enfeeble Me Tenderer
            19, //Let's Get Physical
            20, //Advanced Spellcasting
            21, //Beginner Spellcasting
            22, //Four-Faced
            29, //Eight-Faced
            30  //Hooked on a Healing
        };
    }
}
