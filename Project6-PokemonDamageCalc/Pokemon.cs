using System.Buffers.Text;

namespace Project6_PokemonDamageCalc {

    public class Pokemon {
        int pokedexID, lvl;
        string name;
        int numTypes;
        Type type1, type2;
        double height, weight;
        int hp, atk, def, spatk, spdef;

        private static int CalculateHp(int baseHp, int level)
        {
            // Simplified formula
            return (int)Math.Floor((2.0 * baseHp * level) / 100.0) + level + 10;
        }


        public Pokemon(int lvl, Type t1, Type t2,int basehp, int atk, int def, int spatk, int spdef) {
            this.lvl = lvl;
            this.type1 = t1;
            this.type2 = t2;
            this.hp = CalculateHp(basehp, lvl);
            this.atk = atk;
            this.def = def;
            this.spatk = spatk;
            this.spdef = spdef;
        }
        public Pokemon(int dex, string name, int lvl, Type t1, Type t2, int basehp, int atk, int def, int spatk, int spdef, double height, double weight) {
            this.pokedexID = dex;
            this.name = name;
            this.lvl = lvl;
            this.type1 = t1;
            this.type2 = t2;
            this.hp = CalculateHp(basehp, lvl);
            this.atk = atk;
            this.def = def;
            this.spatk = spatk;
            this.spdef = spdef;
            this.height = height;
            this.weight = weight;
        }
        public int getlvl() {
            return lvl;
        }
        public Type gett1() {
            return type1;
        }
        public Type gett2() {
            return type2;
        }
        public int gethp() {
            return hp;
        }
        public int getatk() {
            return atk;
        }
        public int getdef() {
            return def;
        }
        public int getspatk() {
            return spatk;
        }
        public int getspdef() {
            return spdef;
        }
        public string toString() {
            return "Name: " + this.name
                + "    #" + this.pokedexID
                + "\nLevel: " + this.lvl
                + "\n" + this.type1 + "  " + this.type2
                + "\nHeight: " + height + "m  Weight: " + weight
                + "kg\nHP: " + this.hp
                + "\nAtk: " + this.atk
                + "\nDef: " + this.def
                + "\nSp Atk: " + this.spatk
                + "\nSp Def: " + this.spdef;
        }
    }
}
