using EduKeeper.Infrastructure.DTO;
using PagedList;

namespace EduKeeper.Web.Models
{
    public class PostCollectionModel
    {
        public bool IsHasMore { get; set; }

        public IPagedList<PostDTO> Posts { get; set; }
    }
}