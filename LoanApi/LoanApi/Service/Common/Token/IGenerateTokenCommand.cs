using LoanApi.Domain;

namespace LoanApi.Service.Common.Token
{
    public interface IGenerateTokenCommand
    {
        public string GenerateToken(SystemUser systemUser);
    }
}
