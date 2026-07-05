using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChgManagementSystem.ViewModels
{
    public class CreateLoginViewModel
    {
        public int MemberId { get; set; }

        public string SelectedRole { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}