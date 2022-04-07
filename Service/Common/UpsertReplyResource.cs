using System.ComponentModel;

namespace Service.Common
{
    public class UpsertReplyResource : IBaseResource
    {
        public UpsertReplyResource(int? id)
        {
            Id = id;
        }

        [Description("Database ID of inserted or updated entity")]
        public int? Id { get; set; }
    }
}
