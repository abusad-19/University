namespace University.DAL.Models
{
    public class RolePermissions
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }
    }
}
