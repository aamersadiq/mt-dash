﻿using System.Text.Json.Serialization;

namespace BankDash.Model.Enitity
{
    public class UserRole
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
