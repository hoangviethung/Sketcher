using MainProject.Core.Enums;

namespace MainProject.ServiceAdmin.Model
{
    public class FilterViewModel
    {
        public EntityTypeCollection EntityType { get; set; }
        public LanguageSelectModel LanguageViewModel { get; set; }
        public FatherSelectModel FatherSelectModel { get; set; }
        public string Title { get; set; }
        public bool HasFatherSelect { get; set; }
        public string BaseUrl { get; set; }

        public FilterViewModel() { }

        public FilterViewModel(string baseUrl, LanguageSelectModel languageSelectModel)
        {
            LanguageViewModel = languageSelectModel;
            BaseUrl = baseUrl;
        }

        public FilterViewModel(string baseUrl, LanguageSelectModel languageSelectModel, FatherSelectModel fatherSelectModel)
        {
            LanguageViewModel = languageSelectModel;
            BaseUrl = baseUrl;
            HasFatherSelect = true;
        }
    }
}