using System.Linq;
using IIUSchoolSystem.Core.Repository;

namespace IIUSchoolSystem.Core.Helpers
{
    public static class SettingManager
    {
        private static readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public static string GetSettingValue(string settingName)
        {
            var settings = UnitOfWork.SettingRepository.Get(x => x.SettingName.Equals(settingName)).Select(x => x.Value).FirstOrDefault();
            return settings;
        }
    }
}
