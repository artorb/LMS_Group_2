using System.Threading.Tasks;
using Lms.Core.Entities;

namespace Lms.Web.Service
{
    public interface IActivityService
    {
        Task<string> GetStatusForStudentActivity(Activity clickedActivity, string userId);
    }
}