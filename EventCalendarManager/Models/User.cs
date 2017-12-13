namespace EventCalendarManager.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name can't be an empty string")]
        [MinLength(2, ErrorMessage = "First Name must be between 2 and 10 letters")]
        [MaxLength(10, ErrorMessage = "First Name must be between 2 and 10 letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name can't be an empty string")]
        [MinLength(2, ErrorMessage = "Last Name must be between 2 and 10 letters")]
        [MaxLength(10, ErrorMessage = "Last Name must be between 2 and 10 letters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email can't be an empty string")]
        [MinLength(5, ErrorMessage = "Email must be at least 5 letters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be an empty string")]
        public string Password { get; set; }

        public bool IsLogged { get; set; } = false;

        public List<Event> Events { get; set; } = new List<Event>();
    }
}
