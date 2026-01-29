namespace Project6_PokemonDamageCalc {
    public class TypeChart {

        public static double getEffective(Type a, Type d) {

            if (a == Type.normal) {
                if (d == Type.rock || d == Type.steel)
                    return 0.5;
                else if (d == Type.ghost)
                    return 0;
                else
                    return 1;
            } else if (a == Type.fire) {
                if (d == Type.grass || d == Type.ice || d == Type.bug || d == Type.steel)
                    return 2;
                else if (d == Type.water || d == Type.fire || d == Type.rock || d == Type.dragon)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.water) {
                if (d == Type.fire || d == Type.ground || d == Type.rock)
                    return 2;
                else if (d == Type.water || d == Type.grass || d == Type.dragon)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.electric) {
                if (d == Type.water || d == Type.flying)
                    return 2;
                else if (d == Type.electric || d == Type.dragon)
                    return 0.5;
                else if (d == Type.ground)
                    return 0;
                else
                    return 1;
            } else if (a == Type.grass) {
                if (d == Type.water || d == Type.ground || d == Type.rock)
                    return 2;
                else if (d == Type.fire || d == Type.grass || d == Type.dragon || d == Type.poison || d == Type.flying || d == Type.bug || d == Type.steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.ice) {
                if (d == Type.grass || d == Type.ground || d == Type.flying || d == Type.dragon)
                    return 2;
                else if (d == Type.fire || d == Type.water || d == Type.ice || d == Type.steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.fighting) {
                if (d == Type.normal || d == Type.ice || d == Type.rock || d == Type.dark || d == Type.steel)
                    return 2;
                else if (d == Type.poison || d == Type.flying || d == Type.psychic || d == Type.bug || d == Type.fairy)
                    return 0.5;
                else if (d == Type.ghost)
                    return 0;
                else
                    return 1;
            } else if (a == Type.poison) {
                if (d == Type.grass || d == Type.fairy)
                    return 2;
                else if (d == Type.poison || d == Type.ground || d == Type.rock || d == Type.ghost)
                    return 0.5;
                else if (d == Type.steel)
                    return 0;
                else
                    return 1;
            } else if (a == Type.ground) {
                if (d == Type.fire || d == Type.electric || d == Type.poison || d == Type.fairy)
                    return 2;
                else if (d == Type.grass || d == Type.bug)
                    return 0.5;
                else if (d == Type.flying)
                    return 0;
                else
                    return 1;
            } else if (a == Type.flying) {
                if (d == Type.grass || d == Type.fighting || d == Type.bug)
                    return 2;
                else if (d == Type.electric || d == Type.rock || d == Type.steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.psychic) {
                if (d == Type.fighting || d == Type.poison)
                    return 2;
                else if (d == Type.psychic || d == Type.steel)
                    return 0.5;
                else if (d == Type.dark)
                    return 0;
                else
                    return 1;
            } else if (a == Type.bug) {
                if (d == Type.bug || d == Type.psychic || d == Type.dark)
                    return 2;
                else if (d == Type.fire || d == Type.fighting || d == Type.poison || d == Type.flying || d == Type.ghost || d == Type.steel || d == Type.fairy)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.rock) {
                if (d == Type.fire || d == Type.ice || d == Type.flying || d == Type.bug)
                    return 2;
                else if (d == Type.fighting || d == Type.ground || d == Type.steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.ghost) {
                if (d == Type.bug || d == Type.ghost)
                    return 2;
                else if (d == Type.dark)
                    return 0.5;
                else if (d == Type.normal)
                    return 0;
                else
                    return 1;
            } else if (a == Type.dragon) {
                if (d == Type.dragon)
                    return 2;
                else if (d == Type.steel)
                    return 0.5;
                else if (d == Type.fairy)
                    return 0;
                else
                    return 1;
            } else if (a == Type.dark) {
                if (d == Type.psychic || d == Type.ghost)
                    return 2;
                else if (d == Type.fighting || d == Type.dark || d == Type.fairy)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.steel) {
                if (d == Type.ice || d == Type.rock || d == Type.fairy)
                    return 2;
                else if (d == Type.fire || d == Type.water || d == Type.electric || d == Type.steel)
                    return 0.5;
                else
                    return 1;
            } else if (a == Type.fairy) {
                if (d == Type.fighting || d == Type.dark || d == Type.dragon)
                    return 2;
                else if (d == Type.fire || d == Type.poison || d == Type.steel)
                    return 0.5;
                else
                    return 1;
            } else {
                return 1;
            }
        }
    }
}
