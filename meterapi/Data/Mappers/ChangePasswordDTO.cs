namespace meterapi.Data.Mappers
{
    public class ChangePasswordDTO
    {
        public string newPassword { get; set; }
        public string oldPassword { get; set; }

        public string email { get; set; }
    }
}
