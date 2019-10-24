namespace MainProject.ServiceAdmin.Model.StringResource
{
    public class StringResourceValueModel
    {
        public long Id { get; set; }

        public string KeyName { get; set; }

        public string Value { get; set; }

        public StringResourceValueModel() { }

        public StringResourceValueModel(Core.StringResourceValue entity)
        {
            Id = entity.Id;
            Value = entity.Value;
            KeyName = entity.Key.Name;
        }

        public static void ToEntity(StringResourceValueModel model, ref Core.StringResourceValue entity)
        {
            entity.Value = model.Value;
        }
    }
}
