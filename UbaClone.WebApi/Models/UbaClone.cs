namespace UbaClone.WebApi.Models
{
    public class UbaClone
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = null!;
        public required string Password { get; set; }
        public required int Pin { get; set; }
        public int AccountNumber { get; set; }
        public int Balance { get; set; }
    }
}
