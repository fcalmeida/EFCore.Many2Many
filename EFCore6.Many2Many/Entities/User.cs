namespace EFCore6.Many2Many.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
