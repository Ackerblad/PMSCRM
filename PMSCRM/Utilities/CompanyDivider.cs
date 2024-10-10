namespace PMSCRM.Utilities
{
    public class CompanyDivider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyDivider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCompanyId()
        {
            var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CompanyId");
            if (claim != null)
            {
                return Guid.Parse(claim.Value);
            }
            throw new UnauthorizedAccessException("Company ID not found in claims.");
        }
    }
}
