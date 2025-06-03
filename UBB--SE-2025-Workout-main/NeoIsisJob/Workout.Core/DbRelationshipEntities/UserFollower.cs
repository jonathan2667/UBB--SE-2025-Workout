namespace ServerLibraryProject.DbRelationshipEntities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserFollowers")]
    public class UserFollower
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("follower_id")]
        public long FollowerId { get; set; }
        public UserFollower() { }
        public UserFollower(long userId, long whoToFollow) { 
            UserId = userId;
            FollowerId = whoToFollow;
        }
    }
}
