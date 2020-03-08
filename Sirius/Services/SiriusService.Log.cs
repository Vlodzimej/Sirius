using System;
using System.Collections.Generic;
using Sirius.Helpers;
using Sirius.Models;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        public IEnumerable<Log> GetLogs()
        {
            var logs = _unitOfWork.LogRepository.Get();
            return logs;
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