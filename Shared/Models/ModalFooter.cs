namespace Shared.Models
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "Xác nhận";
        public string CancelButtonText { get; set; } = "Hủy bỏ";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
    }
}