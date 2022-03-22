using System.Collections.Generic;

namespace EFCore3.Many2Many.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();
    }
}
