namespace University.DAL.Models
{
    public class CertificateWithdrawApprovalHistory
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public string? ApplicantType { get; set; }
        public string? Department { get; set; }
        public DateTime RequestCreated { get; set; }
        public string? DepartmentalApprove { get; set; }
        public string? SyndicateApprove { get; set; }
        public DateTime? RecievedDate { get; set; }
        public int RequestStatus { get; set; }
    }
}
