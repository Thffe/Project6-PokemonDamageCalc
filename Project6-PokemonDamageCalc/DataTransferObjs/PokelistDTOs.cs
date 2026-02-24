namespace Project6_PokemonDamageCalc.DataTransferObjs
{
    public class PokelistDTOs
    {
        // One entry (attacker/defender pair + move)
        public record CalcEntryDTO(
            string attackerName,
            string defenderName,
            string moveType,
            string category,
            int power,
            int attackerLevel,
            int defenderLevel
        );

        // Save entry into slot 0/1/2
        public record SaveEntryDTO(
            int entryIndex,
            CalcEntryDTO entry
        );
    }
}