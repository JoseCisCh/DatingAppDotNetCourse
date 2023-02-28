using System;

namespace API.Entities {

    public class AppUser {
        public int Id { get; set; }

        //[Required] This would be to make the Username required to create a row in the table.
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set;}
        public byte[] PasswordSalt { get; set;}
    }
}