using System.ComponentModel.DataAnnotations.Schema;

namespace EQuickKYC.Domain.Entities
{
    public class RegistrationMaster
    {
        public Guid Id { get; set; }
        public Guid UserMasterId { get; set; }
        public string? Name { get; set; }
        public string? PanNo { get; set; }
        public DateOnly? DOB { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ApplicationNumber { get; set; }
        public string ApplicationPrefix { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool status { get; set; }

    }
}
