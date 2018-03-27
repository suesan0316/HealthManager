using System;
using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface IBodyImageService
    {
        bool RegistBodyImage(string base64String);

        bool UpdateBodyImage(int id, string base64String);

        BodyImageModel GetBodyImage();

        List<BodyImageModel> GetBodyImageList();

        bool CheckExitTargetDayData(DateTime targeTime);
    }
}