namespace Project6_PokemonDamageCalc {

    public class Move {

        int power;
        Type mtype;
        Movecategory category;
        public Move(int p, Type t, Movecategory category) {
            power = p;
            mtype = t;
            this.category = category;
        }
        public int getpower() {
            return power;
        }
        public Type getMoveType() {
            return mtype;
        }
        public Movecategory getcategory() {
            return category;
        }
    }
}
