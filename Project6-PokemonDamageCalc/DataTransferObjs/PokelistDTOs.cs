namespace Project6_PokemonDamageCalc.DataTransferObjs
{
    public class PokelistDTOs
    {
        public record PokelistCreateDTOs
            (
            int accountID,
            int teamID,
            int poke1ID,
            int poke2ID,
            int poke3ID,
            int poke4ID,
            int poke5ID,
            int poke6ID
            );

        public record PokelistReplaceDTOs
            (
            int accountID,
            int teamID,
            int poke1ID,
            int poke2ID,
            int poke3ID,
            int poke4ID,
            int poke5ID,
            int poke6ID
            );
    }
}
