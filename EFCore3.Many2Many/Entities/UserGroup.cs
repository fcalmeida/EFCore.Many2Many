using System.Collections.Generic;

namespace EFCore3.Many2Many.Entities
{
    public class UserGroup
    {
        public int UserID { get; set; }
        public int GroupID { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
