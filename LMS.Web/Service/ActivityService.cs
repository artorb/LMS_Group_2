using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Service
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActivityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GetStatusForStudentActivity(Activity clickedActivity, string userId)
        {
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var documents = await _unitOfWork.DocumentRepository.GetWithIncludesAsync(a => a.Include(x => x.Activity));

            var documentForActivity = documents.FirstOrDefault(a => a.Uploader == UserLoggedIn.Email);
            if (documentForActivity == null)
                return "Not uploaded";
            if (documentForActivity.Activity.Name == clickedActivity.Name)
            {
                if (documentForActivity.UploadDate > clickedActivity.Deadline)
                    return "Delayed";
                if (documentForActivity.UploadDate < clickedActivity.Deadline)
                    return "Uploaded";
            }

            return "Not uploaded";
        }

        public async Task<string> GetStatusForStudentModule(Module clickedModule, string userId)
        {
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var documents =
                await _unitOfWork.DocumentRepository.GetWithIncludesAsync(a =>
                    a.Include(x => x.Activity).ThenInclude(y => y.Module));

            var documentForActivity = documents.FirstOrDefault(a => a.Uploader == UserLoggedIn.Email);
            if (documentForActivity == null)
                return "Not uploaded";
            if (documentForActivity.Module.Name == clickedModule.Name)
            {
                if (documentForActivity.UploadDate > clickedModule.EndDate)
                    return "Delayed";
                if (documentForActivity.UploadDate < clickedModule.EndDate)
                    return "Uploaded";
            }

            return "Not uploaded";
        }
    }
}