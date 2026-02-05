using System.Reflection.Metadata;

namespace Project6_PokemonDamageCalc
{
    public class Account
    {
        int accountID;
        char username;
        Blob pfp;

        public Account(int accountID, char username, Blob pfp)
        {
            this.accountID=accountID;
            this.username=username;
            this.pfp=pfp;
        }

        public void createAccount(char username)
        {
            this.username=username;
        }

        public void deleteAccount(char username)
        {

        }

        public void login(char username)
        {
        }

        public void logout(char username)
        {
        }

        public void uploadPic(Blob pfp)
        {

        }

        

    }

}
