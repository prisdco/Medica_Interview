namespace Medica_Interview.Models
{
    public class Employee
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string JobTitle { get; set; }
        public string Team { get; set; }
        public string LineManager { get; set; }
        public DateOnly StartDate { get; set; }
        public string ProfilePicture { get; set; }

        public DateTime? DateOfBirthBinding
        {
            get => DateOfBirth.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value.HasValue)
                {
                    DateOfBirth = DateOnly.FromDateTime(value.Value);
                }
            }
        }

        public DateTime? StartDateBinding
        {
            get => StartDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value.HasValue)
                {
                    StartDate = DateOnly.FromDateTime(value.Value);
                }
            }
        }


        public string DateOfBirthFormatted => DateOfBirth.ToString("dd/MM/yyyy");
        public string StartDateFormatted => StartDate.ToString("dd/MM/yyyy");

    }
}
