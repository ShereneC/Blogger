namespace Blogger.Models
{
    public class Profile
    // REVIEW where do we tell it that this is an extension of account?  Ans: On Account.cs  ": Profile"
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}