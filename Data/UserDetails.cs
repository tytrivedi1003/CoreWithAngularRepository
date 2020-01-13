namespace Data
{
    public class UserDetails : BaseEntity
    {
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string AccessLevel { get; set; }
        public string ReadOnly { get; set; }
    }
}
