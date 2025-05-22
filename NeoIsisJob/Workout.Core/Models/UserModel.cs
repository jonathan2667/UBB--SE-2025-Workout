using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workout.Core.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        [Column("UID")]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Username { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        public UserModel()
        {
            UserWorkouts = new List<UserWorkoutModel>();
            UserClasses = new List<UserClassModel>();
            Rankings = new List<RankingModel>();
        }

        public UserModel(int id)
        {
            ID = id;
            UserWorkouts = new List<UserWorkoutModel>();
            UserClasses = new List<UserClassModel>();
            Rankings = new List<RankingModel>();
        }

        // Navigation properties
        public virtual ICollection<UserWorkoutModel> UserWorkouts { get; set; }
        public virtual ICollection<UserClassModel> UserClasses { get; set; }
        public virtual ICollection<RankingModel> Rankings { get; set; }

        public ICollection<CartItemModel> CartItems { get; set; }
        public ICollection<WishlistItemModel> WishlistItems { get; set; }
        public ICollection<OrderModel> Orders { get; set; }
    }
}
