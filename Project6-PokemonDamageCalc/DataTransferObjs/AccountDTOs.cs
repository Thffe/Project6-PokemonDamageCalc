namespace Project6_PokemonDamageCalc.DataTransferObjs
{
    public class AccountDTOs
    {
        public record accountCreateDTO(string username);
        public record accountReplaceDTO(string username, string pfp = null!, string pfpType=null!);
        public record pfpDTO(string base64, string type);
    }
}
