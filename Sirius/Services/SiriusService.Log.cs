using System;
using System.Collections.Generic;
using System.Linq;
using Sirius.Helpers;
using Sirius.Models;
using Sirius.Models.Enums;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        public object GetLogs()
        {
            var result = _unitOfWork.LogRepository.Get(null, null, "User").Select(item => new
            {
                item.Id,
                item.Content,
                Action = Actions.GetActionAliasByType(item.Action),
                CreateDate = DateConverter.ConvertToStandardString(item.CreateDate),
                User = string.Format("{0} {1}", item.User.LastName, item.User.FirstName)
                 
            });
            return result;
        }

        public void AddLog(string action, string content, Guid userId)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Content = content,
                Action = action,
                CreateDate = DateConverter.ConvertToRTS(DateTime.UtcNow.ToLocalTime()),
                User = _unitOfWork.UserRepository.GetByID(userId)
            };
            _unitOfWork.LogRepository.Insert(log);
            _unitOfWork.Save();
        }
    }
}