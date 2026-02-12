namespace Project6_PokemonDamageCalc.DataTransferObjs
{
    public class PokelistDTOs
    {
        public record PokelistCreateDTOs(
            string accountId,
            string poke1id,
            string poke2id,
            string poke3id,
            string poke4id,
            string poke5id,
            string poke6id
        );

        public record PokelistReplaceDTOs(
            string accountId,
            string poke1id,
            string poke2id,
            string poke3id,
            string poke4id,
            string poke5id,
            string poke6id
        );

        // PATCH: everything optional
        public record PokelistPatchDTOs(
            string? accountId = null,
            string? poke1id = null,
            string? poke2id = null,
            string? poke3id = null,
            string? poke4id = null,
            string? poke5id = null,
            string? poke6id = null
        );
    }
}
