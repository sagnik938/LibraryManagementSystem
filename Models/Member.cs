public class Member
{
    // Properties representing the columns in your database table
    public string FullName { get; set; }
    public string DateOfBirth { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Pincode { get; set; }
    public string FullAddress { get; set; }
    public string MemberId { get; set; }
    public string Password { get; set; }
    public string AccountStatus { get; set; }

    // Constructor for the class (optional)
    public Member(string fullName, string dateOfBirth, string contactNumber, string email, string state, string city, string pincode, string fullAddress, string memberId, string password, string accountStatus)
    {
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        ContactNumber = contactNumber;
        Email = email;
        State = state;
        City = city;
        Pincode = pincode;
        FullAddress = fullAddress;
        MemberId = memberId;
        Password = password;
        AccountStatus = accountStatus;
    }
}
