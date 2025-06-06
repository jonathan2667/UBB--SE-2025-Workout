namespace ServerLibraryProject.DbRelationshipEntities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserFollowers")]
    public class UserFollower
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("follower_id")]
        public int FollowerId { get; set; } // this should probably be int too
        public UserFollower() { }
        public UserFollower(int userId, int whoToFollow) { 
            UserId = userId;
            FollowerId = whoToFollow;
        }
    }
}
