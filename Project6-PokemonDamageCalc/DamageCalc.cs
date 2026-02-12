namespace Project6_PokemonDamageCalc {
    public class DamageCalc {

        
        public static double PrepareCalc(Pokemon Atkr, Pokemon Defr, Move move) {
            double Typemod = TypeChart.getEffective(move.getMoveType(), Defr.gett1())
                             * TypeChart.getEffective(move.getMoveType(), Defr.gett2());

            int atk, def;
            if (move.getcategory() == Movecategory.physical) {
                atk = Atkr.getatk();
                def = Defr.getdef();
            } else {
                atk = Atkr.getspatk();
                def = Defr.getspdef();
            }

            //Divide by zero error prevention
            if(def == 0) {
                def = 1;
            }

            double stab = 1;
            //check for STAB
            if (move.getMoveType() == Atkr.gett1() || move.getMoveType() == Atkr.gett2())
                stab = 1.5;

            double crit = 1;
            var rand = new Random();
            //0 - 7
            int r = rand.Next(8);
            if (r == 0)
                crit = 1.5;

            // 85 - 100
            int rr = rand.Next(85, 101);
            double variance = (double)rr / 100;
            

            double damage = 0;
            damage = calcDamage(Atkr.getlvl(), move.getpower(), atk, def, stab, Typemod, crit, variance);

            //convert to percent of defender's health
            return damage / Defr.gethp();

        }
        public static double calcDamage(int lvl, int power, int atk, int def, double stab, double Type, double crit, double random) {
            //DEFENSE CANNOT BE ZERO
            return ((((lvl * 2 / 5) + 2) * power * atk / def / 50) + 2) * crit * random * stab * Type;
        }
    }
}
