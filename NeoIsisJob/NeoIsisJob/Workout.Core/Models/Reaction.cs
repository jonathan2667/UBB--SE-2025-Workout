﻿namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Workout.Core.Enums;

    [Table("Reactions")]
    public class Reaction
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("post_id")]
        public long PostId { get; set; }

        [Column("reaction_type")]
        public ReactionType Type { get; set; }
    }
}
