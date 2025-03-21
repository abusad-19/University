using University.DAL.Models;

namespace University.MVC.View_Models
{
    public class ShowRequest_ViewModel
    {
        public List<CertificateWithdrawApprovalHistory> RequestList { get; set; }
        public int ApplicantId { get; set; }
        public int deptCode { get; set; }
    }
}
