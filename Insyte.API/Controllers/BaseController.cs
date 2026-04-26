using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

/// <summary>
/// Tüm controller'ların türediği temel sınıf.
/// JWT claim'lerinden kullanıcı bilgilerini çıkarmak için yardımcı metotlar sağlar.
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Mevcut kullanıcının ID'sini JWT claim'inden okur.
    /// </summary>
    protected Guid GetCurrentUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    /// <summary>
    /// Mevcut kullanıcının belirtilen rollerden birine sahip olup olmadığını kontrol eder.
    /// </summary>
    protected bool HasRole(params string[] roles) =>
        roles.Any(r => User.IsInRole(r));
}
