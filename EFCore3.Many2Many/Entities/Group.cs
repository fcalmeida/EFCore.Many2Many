using System.Collections.Generic;

namespace EFCore3.Many2Many.Entities
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();
    }
}
