namespace EventCalendarManager.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title of the event can't be an empty string")]
        [MinLength(3, ErrorMessage = "Title must be between 3 and 20 letters")]
        [MaxLength(20, ErrorMessage = "Title must be between 3 and 20 letters")]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
