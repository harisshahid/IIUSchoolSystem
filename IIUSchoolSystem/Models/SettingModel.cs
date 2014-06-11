using System;
using IIUSchoolSystem.Core.Repository;
using Telerik.Web.Mvc;

namespace IIUSchoolSystem.Models
{
    public class SettingModel
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<long> LastUpdatedByUserId { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public GridModel<SettingModel> Settings { get; set; }
    }

    public class SettingRepository
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public string T(string name, string notes)
        {
            var setting = _unitOfWork.SettingRepository.GetSingle(x => x.SettingName == name);
            if (setting != null)
            {
                if (!string.IsNullOrEmpty(notes))
                {
                    return setting.Description;
                }
                else
                {
                    return setting.Value;
                }
            }
            else
            {
                return "";
            }
        }
    }
}