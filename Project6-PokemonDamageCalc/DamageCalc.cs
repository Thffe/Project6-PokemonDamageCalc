namespace Project6_PokemonDamageCalc {
    public class DamageCalc {

        public void preparecalc() {
            //Need:
            //2 pokemon, a move, 

            //attacker
            Pokemon gengar = new Pokemon(100, Type.ghost, Type.poison, 65, 60, 130, 75);
            //defender
            Pokemon jirachi = new Pokemon(90, Type.psychic, Type.steel, 100, 100, 100, 100);

            //Move: shadow ball
            Move shadowball = new Move(80, Type.ghost, Movecategory.special);


            double Typemod = TypeChart.getEffective(shadowball.getMoveType(), jirachi.gett1()) * TypeChart.getEffective(shadowball.getMoveType(), jirachi.gett2());

            int atk, def;
            if (shadowball.getcategory() == Movecategory.physical) {
                atk = gengar.getatk();
                def = jirachi.getdef();
            } else {
                atk = gengar.getspatk();
                def = jirachi.getspdef();
            }

            double stab = 1;
            //check for STAB
            if (shadowball.getMoveType() == gengar.gett1() || shadowball.getMoveType() == gengar.gett2())
                stab = 1.5;

            double crit = 1;
            var rand = new Random();
            //0 - 7
            int r = rand.Next(8);
            if (r == 0)
                crit = 1.5;
            // 85 - 100
            int rr = rand.Next(85, 101);

            //BUGGED
            double random = rr / 100;
            //temp fix
            random = 1;

            double damage = 0;
            damage = calcDamage(gengar.getlvl(), shadowball.getpower(), atk, def, stab, Typemod, crit, random);

            /*
            richTextBox1.Text = "Damage: " + damage +
                "\nAtkr Level: " + gengar.getlvl() +
                "\nMove pwr: " + shadowball.getpower() +
                "\nAtk used: " + atk +
                "\nDef used: " + def +
                "\nSTAB: " + stab +
                "\nType mod: " + Typemod +
                "\nCrit(roll): " + crit + "(" + r + ")" +
                "\nRandom(roll): " + random + "(" + rr + ")";
            */
        }
        public double eee(Pokemon Atkr, Pokemon Defr, Move move) {
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

            //BUGGED
            double random = rr / 100;
            //temp fix
            random = 1;

            double damage = 0;
            damage = calcDamage(Atkr.getlvl(), move.getpower(), atk, def, stab, Typemod, crit, random);
            return damage;

        }
        public double calcDamage(int lvl, int power, int atk, int def, double stab, double Type, double crit, double random) {
            //DEFENSE CANNOT BE ZERO
            return ((((lvl * 2 / 5) + 2) * power * atk / def / 50) + 2) * crit * random * stab * Type;
        }
    }
}
