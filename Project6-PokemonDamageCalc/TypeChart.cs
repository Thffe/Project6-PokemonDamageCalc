namespace Project6_PokemonDamageCalc {
    public class TypeChart {

        public static double getEffective(Type a, Type d) {

            if (a == Type.Normal) {
                if (d == Type.Rock || d == Type.Steel)
                    return 0.5;
                else if (d == Type.Ghost)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Fire) {
                if (d == Type.Grass || d == Type.Ice || d == Type.Bug || d == Type.Steel)
                    return 2;
                else if (d == Type.Water || d == Type.Fire || d == Type.Rock || d == Type.Dragon)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Water) {
                if (d == Type.Fire || d == Type.Ground || d == Type.Rock)
                    return 2;
                else if (d == Type.Water || d == Type.Grass || d == Type.Dragon)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Electric) {
                if (d == Type.Water || d == Type.Flying)
                    return 2;
                else if (d == Type.Electric || d == Type.Dragon)
                    return 0.5;
                else if (d == Type.Ground)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Grass) {
                if (d == Type.Water || d == Type.Ground || d == Type.Rock)
                    return 2;
                else if (d == Type.Fire || d == Type.Grass || d == Type.Dragon || d == Type.Poison || d == Type.Flying || d == Type.Bug || d == Type.Steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Ice) {
                if (d == Type.Grass || d == Type.Ground || d == Type.Flying || d == Type.Dragon)
                    return 2;
                else if (d == Type.Fire || d == Type.Water || d == Type.Ice || d == Type.Steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Fighting) {
                if (d == Type.Normal || d == Type.Ice || d == Type.Rock || d == Type.Dark || d == Type.Steel)
                    return 2;
                else if (d == Type.Poison || d == Type.Flying || d == Type.Psychic || d == Type.Bug || d == Type.Fairy)
                    return 0.5;
                else if (d == Type.Ghost)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Poison) {
                if (d == Type.Grass || d == Type.Fairy)
                    return 2;
                else if (d == Type.Poison || d == Type.Ground || d == Type.Rock || d == Type.Ghost)
                    return 0.5;
                else if (d == Type.Steel)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Ground) {
                if (d == Type.Fire || d == Type.Electric || d == Type.Poison || d == Type.Fairy)
                    return 2;
                else if (d == Type.Grass || d == Type.Bug)
                    return 0.5;
                else if (d == Type.Flying)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Flying) {
                if (d == Type.Grass || d == Type.Fighting || d == Type.Bug)
                    return 2;
                else if (d == Type.Electric || d == Type.Rock || d == Type.Steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Psychic) {
                if (d == Type.Fighting || d == Type.Poison)
                    return 2;
                else if (d == Type.Psychic || d == Type.Steel)
                    return 0.5;
                else if (d == Type.Dark)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Bug) {
                if (d == Type.Bug || d == Type.Psychic || d == Type.Dark)
                    return 2;
                else if (d == Type.Fire || d == Type.Fighting || d == Type.Poison || d == Type.Flying || d == Type.Ghost || d == Type.Steel || d == Type.Fairy)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Rock) {
                if (d == Type.Fire || d == Type.Ice || d == Type.Flying || d == Type.Bug)
                    return 2;
                else if (d == Type.Fighting || d == Type.Ground || d == Type.Steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Ghost) {
                if (d == Type.Bug || d == Type.Ghost)
                    return 2;
                else if (d == Type.Dark)
                    return 0.5;
                else if (d == Type.Normal)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Dragon) {
                if (d == Type.Dragon)
                    return 2;
                else if (d == Type.Steel)
                    return 0.5;
                else if (d == Type.Fairy)
                    return 0;
                else
                    return 1;
            } else if (a == Type.Dark) {
                if (d == Type.Psychic || d == Type.Ghost)
                    return 2;
                else if (d == Type.Fighting || d == Type.Dark || d == Type.Fairy)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Steel) {
                if (d == Type.Ice || d == Type.Rock || d == Type.Fairy)
                    return 2;
                else if (d == Type.Fire || d == Type.Water || d == Type.Electric || d == Type.Steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.Fairy) {
                if (d == Type.Fighting || d == Type.Dark || d == Type.Dragon)
                    return 2;
                else if (d == Type.Fire || d == Type.Poison || d == Type.Steel)
                    return 0.5;
                else
                    return 1;
            } else {
                return 1;
            }
        }
    }
}
