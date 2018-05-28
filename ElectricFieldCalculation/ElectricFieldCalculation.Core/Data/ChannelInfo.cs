
namespace ElectricFieldCalculation.Core.Data
{
    public class ChannelInfo
    {

        public ChannelInfo(string name, FieldComponent cmp) {
            Name = name;
            Component = cmp;
        }

        public string Name { get; }
        public FieldComponent Component { get; }
    }
}
