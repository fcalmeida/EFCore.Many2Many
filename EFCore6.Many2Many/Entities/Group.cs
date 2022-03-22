namespace EFCore6.Many2Many.Entities
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
