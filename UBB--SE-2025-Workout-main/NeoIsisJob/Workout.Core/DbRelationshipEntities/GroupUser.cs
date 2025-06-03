namespace ServerLibraryProject.DbRelationshipEntities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GroupUsers")]
    public class GroupUser
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("group_id")]
        public long GroupId { get; set; }
    }
}
