namespace BankAccount.Models
{
    public class Owner
    {
        public int IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public static int OwnersLimit { 
            get
            {
                return 2;
            } 
        }

        public Owner(int idNumber, string firstName, string lastName)
        {
            IdNumber = idNumber;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}