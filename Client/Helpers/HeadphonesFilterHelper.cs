using Shared.Models.Enums;
using System.Xml;

namespace Client.Helpers
{
    public class HeadphonesFilterHelper
    {
        public List<HeadphoneType> GetHeadphoneTypes() 
        {
            return Enum.GetValues(typeof(HeadphoneType)).Cast<HeadphoneType>().ToList();
        }
    }
}
